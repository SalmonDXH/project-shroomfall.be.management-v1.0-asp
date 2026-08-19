using Domain.DomainException;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ResponseCode;

namespace Infrastructure.Configuration
{
    public static class PersistenceConfiguration
    {
        #region Attributes
        #endregion

        #region Properties
        #endregion

        #region Methods
        public static IServiceCollection AddPersistenceConfiguration(
            this IServiceCollection services)
        {
            // SQL SERVER
            var sqlConnection = Environment.GetEnvironmentVariable("SQL_CONNECTION_STRING");
            if (string.IsNullOrWhiteSpace(sqlConnection))
                throw new InternalException(
                    InfrastructureCode.PersistenceConfigurationCode.SqlConnectionStringMissing,
                    "Critical infrastructure configuration missing. Environment variable 'SQL_CONNECTION_STRING' was not found.");

            services.AddDbContext<RelationalDB>(options =>
                options.UseSqlServer(sqlConnection));

            return services;
        }
        #endregion
    }
}