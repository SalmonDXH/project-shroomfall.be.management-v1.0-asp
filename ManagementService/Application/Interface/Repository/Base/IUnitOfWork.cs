namespace Application.Interface.Repository.Base
{
    public interface IUnitOfWork
    {
        T GetRepository<T>() where T : IRepository;
        Task BeginTransactionAsync();
        Task<int> CommitAsync();
        Task<int> SaveChangesAsync();
    }

    public interface IRepository
    {

    }
}