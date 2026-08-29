using Application.Mapper;
using Microsoft.Extensions.DependencyInjection;
namespace Application.Configuration
{
    public static class MapperConfiguration
    {
        #region Attributes
        #endregion

        #region Properties
        #endregion

        #region Methods
        public static IServiceCollection AddMapperConfiguration(
            this IServiceCollection services)
        {
            // MAPPER
            services.AddAutoMapper(cfg => { cfg.AddProfile<CommonMapper>(); });
            services.AddAutoMapper(cfg => { cfg.AddProfile<EntityDomainMapper>(); });
            services.AddAutoMapper(cfg => { cfg.AddProfile<IdentityDomainMapper>(); });
            services.AddAutoMapper(cfg => { cfg.AddProfile<LocalizationDomainMapper>(); });
            services.AddAutoMapper(cfg => { cfg.AddProfile<MetaDomainMapper>(); });
            services.AddAutoMapper(cfg => { cfg.AddProfile<VersioningMapper>(); });
            services.AddAutoMapper(cfg => { cfg.AddProfile<WorldDomainMapper>(); });

            return services;
        }
        #endregion
    }
}
