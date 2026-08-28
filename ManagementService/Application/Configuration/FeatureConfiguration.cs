using Application.Feature;
using Application.Feature.Abstraction;
using Application.Feature.Design.Command;
using Application.Feature.Design.Handler;
using Application.Feature.Identity.Command;
using Application.Feature.Identity.Handler;
using Contract.DTO.Abstraction;
using Contract.DTO.Definition.EntityDomain.Component;
using Contract.DTO.Definition.LocalizationDomain;
using Contract.DTO.Definition.MetaDomain;
using Contract.DTO.Definition.WorldDomain;
using Contract.DTO.Feature.Design.Response;
using Contract.DTO.Feature.Identity.Response;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Configuration
{
    public static class FeatureConfiguration
    {
        #region Attributes
        #endregion

        #region Properties
        #endregion

        #region Methods
        public static IServiceCollection AddFeatureConfiguration(
            this IServiceCollection services)
        {
            // Core
            services.AddScoped<IDispatcher, Dispatcher>();

            // Identity
            services.AddScoped<IHandler<LoginCommand, TokenDTO>, LoginHandler>();
            services.AddScoped<IHandler<RefreshTokenCommand, TokenDTO>, RefreshTokenHandler>();
            services.AddScoped<IHandler<RegisterCommand, TokenDTO>, RegisterHandler>();
            services.AddScoped<IHandler<SteamAuthCommand, TokenDTO>, SteamAuthHandler>();
            services.AddScoped<IHandler<UpdateProfileCommand>, UpdateProfileHandler>();

            // Design
            services.AddScoped<IHandler<FetchCombatRunDefinitionCommand, PagedResponseDTO<CombatRunDefinitionDTO>>, FetchCombatRunDefinitionHandler>();
            services.AddScoped<IHandler<FetchEffectDefinitionCommand, PagedResponseDTO<EffectDefinitionDTO>>, FetchEffectDefinitionHandler>();
            services.AddScoped<IHandler<FetchEntityDefinitionCommand, PagedResponseDTO<EntityDefinitionDTO>>, FetchEntityDefinitionHandler>();
            services.AddScoped<IHandler<FetchEntityDefinitionDetailCommand, EntityDefinitionDTO>, FetchEntityDefinitionDetailHandler>();
            services.AddScoped<IHandler<FetchItemDefinitionCommand, PagedResponseDTO<ItemDefinitionDTO>>, FetchItemDefinitionHandler>();
            services.AddScoped<IHandler<FetchLocaleCommand, List<LocaleDTO>>, FetchLocaleHandler>();
            services.AddScoped<IHandler<FetchLocalizationEntryCommand, PagedResponseDTO<LocalizationEntryDTO>>, FetchLocalizationEntryHandler>();
            services.AddScoped<IHandler<FetchRoomDefinitionCommand, PagedResponseDTO<RoomDefinitionDTO>>, FetchRoomDefinitionHandler>();
            services.AddScoped<IHandler<ImportCombatRunDefinitionCommand>, ImportCombatRunDefinitionHandler>();
            services.AddScoped<IHandler<ImportEffectDefinitionCommand>, ImportEffectDefinitionHandler>();
            services.AddScoped<IHandler<ImportEntityDefinitionCommand>, ImportEntityDefinitionHandler>();
            services.AddScoped<IHandler<ImportItemDefinitionCommand>, ImportItemDefinitionHandler>();
            services.AddScoped<IHandler<ImportRoomDefinitionCommand>, ImportRoomDefinitionHandler>();
            services.AddScoped<IHandler<UpdateDefinitionCommand>, UpdateDefinitionHandler>();
            services.AddScoped<IHandler<UpdateLocalizationEntryCommand>, UpdateLocalizationEntryHandler>();
            services.AddScoped<IHandler<UpsertCombatRunDefinitionCommand>, UpsertCombatRunDefinitionHandler>();
            services.AddScoped<IHandler<UpsertEffectDefinitionCommand>, UpsertEffectDefinitionHandler>();
            services.AddScoped<IHandler<UpsertEntityDefinitionCommand>, UpsertEntityDefinitionHandler>();
            services.AddScoped<IHandler<UpsertItemDefinitionCommand>, UpsertItemDefinitionHandler>();

            return services;
        }
        #endregion
    }
}
