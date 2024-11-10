using System.Reflection;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Messaging.MassTransit;

public static class DependencyInjection
{
    public static IServiceCollection AddMessageBroker
        (this IServiceCollection services, IConfiguration configuration, Assembly? assembly = null)
    {
        services.AddMassTransit(config =>
        {
            config.SetKebabCaseEndpointNameFormatter();

            if (assembly != null)
            {
                config.AddConsumers(assembly);
            }

            IConfigurationSection messageBrokerConfig = configuration.GetSection("MessageBroker");

            config.UsingRabbitMq((context, configurator) =>
            {
                configurator.Host(new Uri(messageBrokerConfig["Host"]!), host =>
                {
                    host.Username(messageBrokerConfig["UserName"]!);
                    host.Password(messageBrokerConfig["Password"]!);
                });
                configurator.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}
