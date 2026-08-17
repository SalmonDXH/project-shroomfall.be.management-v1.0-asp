using Application.Interface.Repository;
using Domain.EntityDomain.Component;
using Infrastructure.Persistence;
using Infrastructure.Repository.Base;

namespace Infrastructure.Repository
{
    public class LifetimeDefinitionRepository : DefinitionRepository<LifetimeDefinition>, ILifetimeDefinitionRepository
    {
        #region Attributes
        #endregion

        #region Properties
        #endregion

        public LifetimeDefinitionRepository(RelationalDB context) : base(context) { }

        #region Methods
        #endregion
    }
}