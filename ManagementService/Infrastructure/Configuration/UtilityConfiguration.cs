using Application.Interface.Utility;
using Domain.DomainException;
using Infrastructure.Utility;
using Microsoft.Extensions.DependencyInjection;
using ResponseCode;

namespace Infrastructure.Configuration
{
    public static class UtilityConfiguration
    {
        #region Attributes
        #endregion

        #region Properties
        #endregion

        #region Methods
        public static IServiceCollection AddUtilityConfiguration(
            this IServiceCollection services)
        {
            // GOOGLE TOKEN VALIDATOR
            services.AddHttpClient<GoogleTokenValidator>();

            services.AddScoped<IGoogleTokenValidator, GoogleTokenValidator>(sp =>
            {
                var googleClientId = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_ID");
                if (string.IsNullOrWhiteSpace(googleClientId))
                    throw new InternalException(
                        InfrastructureCode.UtilityConfigurationCode.GoogleClientIdMissing,
                        "Critical infrastructure configuration missing. Environment variable 'GOOGLE_CLIENT_ID' was not found.");

                return new GoogleTokenValidator(googleClientId);
            });

            // JWT TOKEN GENERATOR
            services.AddSingleton<ITokenGenerator, TokenGenerator>(sp =>
            {
                var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY");
                if (string.IsNullOrWhiteSpace(jwtKey))
                    throw new InternalException(
                        InfrastructureCode.UtilityConfigurationCode.JwtKeyMissing,
                        "Critical infrastructure configuration missing. Environment variable 'JWT_KEY' was not found.");

                var issuer = Environment.GetEnvironmentVariable("JWT_ISSUER");
                if (string.IsNullOrWhiteSpace(issuer))
                    throw new InternalException(
                        InfrastructureCode.UtilityConfigurationCode.JwtIssuerMissing,
                        "Critical infrastructure configuration missing. Environment variable 'JWT_ISSUER' was not found.");

                var audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE");
                if (string.IsNullOrWhiteSpace(audience))
                    throw new InternalException(
                        InfrastructureCode.UtilityConfigurationCode.JwtAudienceMissing,
                        "Critical infrastructure configuration missing. Environment variable 'JWT_AUDIENCE' was not found.");

                return new TokenGenerator(jwtKey, issuer, audience);
            });

            return services;
        }
        #endregion
    }
}