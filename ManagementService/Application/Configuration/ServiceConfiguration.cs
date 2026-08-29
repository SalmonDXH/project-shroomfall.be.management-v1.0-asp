using Application.Service.DesignService;
using Application.Service.IdentityService;
using Application.Services.DesignService;
using Microsoft.Extensions.DependencyInjection;
namespace Application.Configuration
{
    public static class ServiceConfiguration
    {
        #region Attributes
        #endregion

        #region Properties
        #endregion

        #region Methods
        public static IServiceCollection AddServiceConfiguration(
            this IServiceCollection services)
        {
            // Identity
            services.AddSingleton<TokenService>();

            // Design
            services.AddScoped<CacheBuilder>();
            services.AddScoped<CombatRunDefinitionService>();
            services.AddSingleton<ComponentDiscoveryRegistry>();
            services.AddScoped<DefinitionComponentFactory>();
            services.AddScoped<EffectDefinitionService>();
            services.AddScoped<EntityDefinitionService>();
            services.AddScoped<ItemDefinitionService>();
            services.AddScoped<LocalizationEntryFactory>();

            return services;
        }
        #endregion
    }
}
