using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infrastructure.Persistence
{
    public class ManagementDBContextFactory : IDesignTimeDbContextFactory<ManagementDBContext>
    {
        #region Attributes
        #endregion

        #region Properties
        #endregion

        public ManagementDBContextFactory()
        {

        }

        #region Methods
        public ManagementDBContext CreateDbContext(
            string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ManagementDBContext>();

            optionsBuilder.UseSqlServer(
                "Server=localhost;Database=GameServiceDB;Trusted_Connection=True;TrustServerCertificate=True;"
            );

            return new ManagementDBContext(optionsBuilder.Options);
        }
        #endregion
    }
}