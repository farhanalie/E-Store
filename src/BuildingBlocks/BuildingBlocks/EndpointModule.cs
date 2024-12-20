﻿using Carter;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace BuildingBlocks;

public abstract class EndpointModule : ICarterModule
{
    public abstract void AddRoutes(IEndpointRouteBuilder app);

    protected static async Task<IResult> Handle<TResponse>(IRequest<ErrorOr<TResponse>> query, ISender sender,
        Func<TResponse, IResult>? onSuccess = null)
    {
        var result = await sender.Send(query);

        onSuccess ??= Results.Ok;

        return result.Match(
            response => onSuccess(response),
            ProblemResult);
    }

    public static IResult ProblemResult(List<Error> errors)
    {
        return Results.Problem(ProblemDetails(errors));
    }

    public static ProblemDetails ProblemDetails(List<Error> errors)
    {
        if (errors.Count is 0)
        {
            return TypedResults.Problem().ProblemDetails;
        }

        if (errors.All(error => error.Type == ErrorType.Validation))
        {
            return ValidationProblemResult(errors).ProblemDetails;
        }

        return ProblemResult(errors[0]).ProblemDetails;
    }

    public static ProblemHttpResult ProblemResult(Error error)
    {
        var statusCode = error.Type switch
        {
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Unauthorized => StatusCodes.Status403Forbidden,
            _ => StatusCodes.Status500InternalServerError
        };

        return TypedResults.Problem(statusCode: statusCode, title: error.Description);
    }

    private static ValidationProblem ValidationProblemResult(IEnumerable<Error> errors)
    {
        var validationErrors = errors
            .GroupBy(error => error.Code)
            .ToDictionary(
                group => group.Key,
                group => group.Select(error => error.Description).ToArray());

        return TypedResults.ValidationProblem(validationErrors);
    }
}