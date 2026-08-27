using Application.Feature.Abstraction;
using Application.Feature.Design.Command;
using Application.Interface.Repository.Base;
using Application.Service.DesignService;

namespace Application.Feature.Design.Handler
{
    public class UpsertEffectDefinitionHandler : IHandler<UpsertEffectDefinitionCommand>
    {
        #region Attributes
        private readonly IUnitOfWork uow;
        private readonly EffectDefinitionService effectDefinitionService;
        #endregion

        #region Properties
        #endregion

        public UpsertEffectDefinitionHandler(
            IUnitOfWork uow,
            EffectDefinitionService effectDefinitionService)
        {
            this.uow = uow;
            this.effectDefinitionService = effectDefinitionService;
        }

        #region Methods
        public async Task Handle(
            UpsertEffectDefinitionCommand command)
        {
            await effectDefinitionService.UpsertWithoutSave(command.DTO);
            await uow.SaveChangesAsync();
        }
        #endregion
    }
}