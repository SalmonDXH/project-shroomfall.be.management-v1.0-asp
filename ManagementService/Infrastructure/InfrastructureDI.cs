using Infrastructure.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class InfrastructureDI
    {
        #region Attributes
        #endregion

        #region Properties
        #endregion

        #region Methods
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            // ─────────────────────────────
            // MESSAGING
            // ─────────────────────────────
            services.AddMessagingConfiguration();

            // ─────────────────────────────
            // PERSISTENCES
            // ─────────────────────────────
            services.AddPersistenceConfiguration();

            // ─────────────────────────────
            // REPOSITORIES
            // ─────────────────────────────
            services.AddRepositoryConfiguration();

            // ─────────────────────────────
            // UTILITY
            // ─────────────────────────────
            services.AddUtilityConfiguration();

            return services;
        }
        #endregion
    }
}