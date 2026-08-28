using Application.Feature.Abstraction;
using Application.Feature.Design.Command;
using Application.Interface.Repository.Base;
using Application.Services.DesignService;

namespace Application.Feature.Design.Handler
{
    public class UpsertEntityDefinitionHandler : IHandler<UpsertEntityDefinitionCommand>
    {
        #region Attributes
        private readonly IUnitOfWork uow;
        private readonly EntityDefinitionService entityDefinitionService;
        #endregion

        #region Properties
        #endregion

        public UpsertEntityDefinitionHandler(
            IUnitOfWork uow,
            EntityDefinitionService entityDefinitionService)
        {
            this.uow = uow;
            this.entityDefinitionService = entityDefinitionService;
        }

        #region Methods
        public async Task Handle(
            UpsertEntityDefinitionCommand command)
        {
            await entityDefinitionService.UpsertWithoutSave(command.DTO);
            await uow.SaveChangesAsync();
        }
        #endregion
    }
}