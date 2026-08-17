using Application.Interface.Repository.Base;
using Domain;

namespace Application.Interface.Repository
{
    public interface IDefinitionVersionLogRepository : IGenericRepository<DefinitionVersionLog>, IRepository
    {
        Task<DefinitionVersionLog?> GetLatest(
            string key);
    }
}