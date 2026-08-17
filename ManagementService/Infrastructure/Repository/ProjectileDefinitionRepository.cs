using Application.Interface.Repository;
using Domain.EntityDomain.Component;
using Infrastructure.Persistence;
using Infrastructure.Repository.Base;

namespace Infrastructure.Repository.Relational
{
    public class ProjectileDefinitionRepository : DefinitionRepository<ProjectileDefinition>, IProjectileDefinitionRepository
    {
        #region Attributes
        #endregion

        #region Properties
        #endregion

        public ProjectileDefinitionRepository(RelationalDB context) : base(context) { }

        #region Methods
        #endregion
    }
}