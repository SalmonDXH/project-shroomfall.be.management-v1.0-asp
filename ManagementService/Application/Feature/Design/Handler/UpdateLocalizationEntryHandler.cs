using Application.Feature.Abstraction;
using Application.Feature.Design.Command;
using Application.Interface.Repository;
using Application.Interface.Repository.Base;
using Domain.DomainException;
using ResponseCode;

namespace Application.Feature.Design.Handler
{
    public class UpdateLocalizationEntryHandler : IHandler<UpdateLocalizationEntryCommand>
    {
        #region Attributes
        private readonly IUnitOfWork uow;
        #endregion

        #region Properties
        #endregion

        public UpdateLocalizationEntryHandler(
            IUnitOfWork uow)
        {
            this.uow = uow;
        }

        #region Methods
        public async Task Handle(
            UpdateLocalizationEntryCommand command)
        {
            var dto = command.DTO;

            // Validate entry existence
            var localeRepo = uow.GetRepository<ILocaleRepository>();
            var entry = await localeRepo.GetByKeyAsync(dto.LocaleCode, dto.Key);
            if (entry == null)
                throw new BadRequest(
                    ApplicationCode.DesignHandlerCode.LocalizationEntryNotFound,
                    $"Localization key '{dto.Key}' was not found under locale '{dto.LocaleCode}'.");

            // Apply domain
            entry.Update(dto.Value, dto.Description);

            // Apply persistence
            await uow.SaveChangesAsync();
        }
        #endregion
    }
}