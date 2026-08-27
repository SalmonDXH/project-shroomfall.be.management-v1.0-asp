using Application.Feature.Abstraction;
using Application.Feature.Design.Command;
using Application.Interface.Repository.Base;
using Application.Service.DesignService;

namespace Application.Feature.Design.Handler
{
    public class UpsertCombatRunDefinitionHandler : IHandler<UpsertCombatRunDefinitionCommand>
    {
        #region Attributes
        private readonly IUnitOfWork uow;
        private readonly CombatRunDefinitionService combatRunDefinitionService;
        #endregion

        #region Properties
        #endregion

        public UpsertCombatRunDefinitionHandler(
            IUnitOfWork uow,
            CombatRunDefinitionService combatRunDefinitionService)
        {
            this.uow = uow;
            this.combatRunDefinitionService = combatRunDefinitionService;
        }

        #region Methods
        public async Task Handle(
            UpsertCombatRunDefinitionCommand command)
        {
            await combatRunDefinitionService.UpsertWithoutSave(command.DTO);
            await uow.SaveChangesAsync();
        }
        #endregion
    }
}