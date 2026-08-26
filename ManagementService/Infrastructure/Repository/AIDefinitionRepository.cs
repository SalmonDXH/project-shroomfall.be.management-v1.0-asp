using Application.Interface.Repository;
using Domain.EntityDomain.Component;
using Infrastructure.Persistence;
using Infrastructure.Repository.Base;

namespace Infrastructure.Repository
{
    public class AIDefinitionRepository : DefinitionRepository<AIDefinition>, IAIDefinitionRepository
    {
        #region Attributes
        #endregion

        #region Properties
        #endregion

        public AIDefinitionRepository(ManagementDBContext context) : base(context) { }

        #region Methods
        #endregion
    }
}