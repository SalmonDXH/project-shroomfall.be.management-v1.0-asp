using Application.Interface.Repository.Base;
using Domain.EntityDomain.Component;

namespace Application.Interface.Repository
{
    public interface IAIDefinitionRepository : IDefinitionRepository<AIDefinition>, IRepository
    {

    }
}