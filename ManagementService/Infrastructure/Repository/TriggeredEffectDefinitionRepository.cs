using Application.Interface.Repository;
using Domain.EntityDomain.Component;
using Infrastructure.Persistence;
using Infrastructure.Repository.Base;

namespace Infrastructure.Repository
{
    public class TriggeredEffectDefinitionRepository : DefinitionRepository<TriggeredEffectDefinition>, ITriggeredEffectDefinitionRepository
    {
        #region Attributes
        #endregion

        #region Properties
        #endregion

        public TriggeredEffectDefinitionRepository(RelationalDB context) : base(context) { }

        #region Methods
        #endregion
    }
}