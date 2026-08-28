using Application.Feature.Abstraction;
using Application.Feature.Design.Command;
using Application.Interface.Repository.Base;
using Application.Service.DesignService;

namespace Application.Feature.Design.Handler
{
    public class UpsertItemDefinitionHandler : IHandler<UpsertItemDefinitionCommand>
    {
        #region Attributes
        private readonly IUnitOfWork uow;
        private readonly ItemDefinitionService itemDefinitionService;
        #endregion

        #region Properties
        #endregion

        public UpsertItemDefinitionHandler(
            IUnitOfWork uow,
            ItemDefinitionService itemDefinitionService)
        {
            this.uow = uow;
            this.itemDefinitionService = itemDefinitionService;
        }

        #region Methods
        public async Task Handle(
            UpsertItemDefinitionCommand command)
        {
            await itemDefinitionService.UpsertWithoutSave(command.DTO);
            await uow.SaveChangesAsync();
        }
        #endregion
    }
}