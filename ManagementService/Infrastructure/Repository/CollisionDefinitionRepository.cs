using Application.Interface.Repository;
using Domain.EntityDomain.Component;
using Infrastructure.Persistence;
using Infrastructure.Repository.Base;

namespace Infrastructure.Repository
{
    public class CollisionDefinitionRepository : DefinitionRepository<CollisionDefinition>, ICollisionDefinitionRepository
    {
        #region Attributes
        #endregion

        #region Properties
        #endregion

        public CollisionDefinitionRepository(ManagementDBContext context) : base(context) { }

        #region Methods
        #endregion
    }
}