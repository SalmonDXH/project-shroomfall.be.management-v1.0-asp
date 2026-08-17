using Application.Interface.Repository;
using Domain.EntityDomain.Component;
using Infrastructure.Persistence;
using Infrastructure.Repository.Base;

namespace Infrastructure.Repository
{
    public class AppearanceDefinitionRepository : DefinitionRepository<AppearanceDefinition>, IAppearanceDefinitionRepository
    {
        #region Attributes
        #endregion

        #region Properties
        #endregion

        public AppearanceDefinitionRepository(RelationalDB context) : base(context) { }

        #region Methods
        #endregion
    }
}