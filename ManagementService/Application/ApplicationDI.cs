using Application.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public static class ApplicationDI
    {
        #region Attributes
        #endregion

        #region Properties
        #endregion

        #region Methods
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // ─────────────────────────────
            // FEATURES
            // ─────────────────────────────
            services.AddFeatureConfiguration();

            // ─────────────────────────────
            // SERVICES
            // ─────────────────────────────
            services.AddServiceConfiguration();

            // ─────────────────────────────
            // MAPPERS
            // ─────────────────────────────
            services.AddMapperConfiguration();

            return services;
        }
        #endregion
    }
}
