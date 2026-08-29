using Application.Interface.Messaging.Publisher;
using Domain.DomainException;
using Infrastructure.Messaging.Publisher;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using ResponseCode;

namespace Infrastructure.Configuration
{
    public static class MessagingConfiguration
    {
        #region Attributes
        #endregion

        #region Properties
        #endregion

        #region Methods
        public static IServiceCollection AddMessagingConfiguration(
            this IServiceCollection services)
        {
            // RABBITMQ
            var rabbitHost = Environment.GetEnvironmentVariable("RABBITMQ_HOST");
            var rabbitUsername = Environment.GetEnvironmentVariable("RABBITMQ_USERNAME");
            var rabbitPassword = Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD");

            if (string.IsNullOrWhiteSpace(rabbitHost))
                throw new InternalException(
                    InfrastructureCode.MessagingConfigurationCode.RabbitMqHostMissing,
                    "Critical infrastructure configuration missing. Environment variable 'RABBITMQ_HOST' was not found.");

            if (string.IsNullOrWhiteSpace(rabbitUsername))
                throw new InternalException(
                    InfrastructureCode.MessagingConfigurationCode.RabbitMqUsernameMissing,
                    "Critical infrastructure configuration missing. Environment variable 'RABBITMQ_USERNAME' was not found.");

            if (string.IsNullOrWhiteSpace(rabbitPassword))
                throw new InternalException(
                    InfrastructureCode.MessagingConfigurationCode.RabbitMqPasswordMissing,
                    "Critical infrastructure configuration missing. Environment variable 'RABBITMQ_PASSWORD' was not found.");

            services.AddMassTransit(configurator =>
            {
                configurator.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(rabbitHost, "/", host =>
                    {
                        host.Username(rabbitUsername);
                        host.Password(rabbitPassword);
                    });
                });
            });

            // PUBLISHER
            services.AddScoped<IDefinitionCachePublisher, DefinitionCachePublisher>();
            services.AddScoped<IGameStartupPublisher, GameStartupPublisher>();

            return services;
        }
        #endregion
    }
}