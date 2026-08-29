using Application.Interface.Repository;
using Application.Interface.Repository.Base;
using Infrastructure.Repository;
using Infrastructure.Repository.Base;
using Infrastructure.Repository.Relational;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Configuration
{
    public static class RepositoryConfiguration
    {
        #region Attributes
        #endregion

        #region Properties
        #endregion

        #region Methods
        public static IServiceCollection AddRepositoryConfiguration(
            this IServiceCollection services)
        {
            // UNIT OF WORK
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // ENTITY REPOSITORY
            services.AddScoped<IAIDefinitionRepository, AIDefinitionRepository>();
            services.AddScoped<IAppearanceDefinitionRepository, AppearanceDefinitionRepository>();
            services.AddScoped<ICollisionDefinitionRepository, CollisionDefinitionRepository>();
            services.AddScoped<ICharacteristicDefinitionRepository, CharacteristicDefinitionRepository>();
            services.AddScoped<IInventoryDefinitionRepository, InventoryDefinitionRepository>();
            services.AddScoped<ILifetimeDefinitionRepository, LifetimeDefinitionRepository>();
            services.AddScoped<IProjectileDefinitionRepository, ProjectileDefinitionRepository>();
            services.AddScoped<ITriggeredEffectDefinitionRepository, TriggeredEffectDefinitionRepository>();
            services.AddScoped<IEntityDefinitionRepository, EntityDefinitionRepository>();

            // IDENTITY REPOSITORY
            services.AddScoped<IUserRepository, UserRepository>();

            // LOCALIZATION REPOSITORY
            services.AddScoped<ILocaleRepository, LocaleRepository>();

            // META REPOSITORY
            services.AddScoped<IEffectDefinitionRepository, EffectDefinitionRepository>();
            services.AddScoped<IItemDefinitionRepository, ItemDefinitionRepository>();

            // WORLD REPOSITORY
            services.AddScoped<ICombatRunDefinitionRepository, CombatRunDefinitionRepository>();
            services.AddScoped<IRoomDefinitionRepository, RoomDefinitionRepository>();

            // VERSION REPOSITORY
            services.AddScoped<IDefinitionVersionLogRepository, DefinitionVersionLogRepository>();

            return services;
        }
        #endregion
    }
}