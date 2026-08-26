using Application.Interface.Repository;
using Application.Interface.Repository.Base;
using Domain.LocalizationDomain;

namespace Application.Service.DesignService
{
    public class LocalizationEntryFactory
    {
        #region Attributes
        private readonly IUnitOfWork uow;
        #endregion

        #region Properties
        #endregion

        public LocalizationEntryFactory(
            IUnitOfWork uow)
        {
            this.uow = uow;
        }

        #region Methods
        public async Task PreSavePlaceholderKeysAsync(
            LocalizedText keys)
        {
            // Resolve repository
            var localeRepo = uow.GetRepository<ILocaleRepository>();

            var pendingEntries = new List<LocalizationEntry>();

            // Retrieve existed locale to populate entries
            var activeLocales = await localeRepo.GetAllAsync();
            foreach (var locale in activeLocales)
            {
                pendingEntries.Add(new LocalizationEntry(Guid.NewGuid(), keys.NameKey, locale.Code, string.Empty));
                pendingEntries.Add(new LocalizationEntry(Guid.NewGuid(), keys.DescriptionKey, locale.Code, string.Empty));
            }

            // Apply persistence
            await localeRepo.SaveLocalizationEntriesAsync(pendingEntries);
        }
        #endregion
    }
}