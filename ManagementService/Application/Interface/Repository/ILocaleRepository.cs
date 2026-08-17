using Application.Interface.Repository.Base;
using Domain.LocalizationDomain;

namespace Application.Interface.Repository
{
    public interface ILocaleRepository : IGenericRepository<Locale>, IRepository
    {
        Task<IEnumerable<Locale>> GetAllAsyncWithoutJoined();
        Task<(IEnumerable<LocalizationEntry> Items, int TotalCount)> GetPagedDefinitionsAsync(
            string? searchTerm,
            string localeCode,
            int pageNumber,
            int pageSize);
        Task<LocalizationEntry?> GetByKeyAsync(
            string localeCode,
            string key);
        Task SaveLocalizationEntriesAsync(
            IEnumerable<LocalizationEntry> localizationEntries);
    }
}