using ErrorOr;
using FluentValidation;
using MediatR;

namespace BuildingBlocks.Behaviors;

public class ValidationBehavior<TRequest, TResponse>(IValidator<TRequest>? validator = null)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : IErrorOr
{
    private readonly IValidator<TRequest>? _validator = validator;

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (_validator is null)
        {
            return await next();
        }

        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (validationResult.IsValid)
        {
            return await next();
        }

        var errors = validationResult.Errors
            .ConvertAll(error => Error.Validation(
                error.PropertyName,
                error.ErrorMessage));

        return (dynamic)errors;
    }
}
//public class ValidationBehavior<TRequest, TResponse>
//    (IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, TResponse>
//    where TRequest : IRequest<TResponse>
//    where TResponse : IErrorOr
//{
//    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
//    {
//        if (!validators.Any())
//        {
//            return await next();
//        }

//        var context = new ValidationContext<TRequest>(request);

//        ValidationResult[] validationResults = await Task.WhenAll(
//            validators.Select(v =>
//                v.ValidateAsync(context, cancellationToken)));

//        if (validationResults.All(validationResult => validationResult.IsValid))
//        {
//            return await next();
//        }

//        List<ValidationFailure> validationFailures = validationResults
//            .Where(validationResult => validationResult.Errors.Any())
//            .SelectMany(validationResult => validationResult.Errors)
//            .ToList();

//        return TryCreateResponseFromErrors(validationFailures, out var response)
//            ? response!
//            : throw new ValidationException(validationFailures);
//    }

//    private static bool TryCreateResponseFromErrors(List<ValidationFailure> validationFailures, out TResponse? response)
//    {
//        var errors = validationFailures.ConvertAll(x => Error.Validation(
//            x.PropertyName,
//            x.ErrorMessage, new Dictionary<string, object>
//            {
//                {
//                    nameof(x.Severity), x.Severity
//                }
//            }));

//        response = (TResponse?)typeof(TResponse)
//            .GetMethod(
//                nameof(ErrorOr<object>.From),
//                BindingFlags.Static | BindingFlags.Public,
//                new[] { typeof(List<Error>) })?
//            .Invoke(null, new object?[] { errors });

//        return response is not null;
//    }
//}
