using Application.Interface.Repository.Base;
using Domain.DomainException;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using ResponseCode;
using System.Runtime.InteropServices;

namespace Infrastructure.Repository.Base
{
    public class UnitOfWork : IUnitOfWork
    {
        #region Attributes
        private readonly RelationalDB context;
        private readonly IServiceProvider provider;
        private readonly Dictionary<Type, object> repositories = new();

        private IDbContextTransaction? transaction;
        #endregion

        #region Properties
        #endregion

        public UnitOfWork(
            RelationalDB context,
            IServiceProvider provider)
        {
            this.context = context;
            this.provider = provider;
        }

        #region Methods
        public T GetRepository<T>() where T : IRepository
        {
            var type = typeof(T);

            if (!repositories.TryGetValue(type, out var repo))
            {
                repo = provider.GetRequiredService<T>();
                repositories[type] = repo!;
            }

            return (T)repo!;
        }

        public async Task BeginTransactionAsync()
        {
            if (transaction == null)
                transaction = await context.Database.BeginTransactionAsync();
        }

        public async Task<int> CommitAsync()
        {
            if (transaction == null)
                throw new InternalException(
                    InfrastructureCode.RelationalUoWCode.NoTransaction,
                    "An attempt was made to commit a transaction before one was started.");

            try
            {
                await context.SaveChangesAsync();
                await transaction.CommitAsync();
                return 1;
            }
            catch (Exception ex)
            {
                await RollbackAsync();

                throw new InternalException(
                    InfrastructureCode.RelationalUoWCode.CommitFailed,
                    $"Failed to commit database transaction and save changes: {ex.Message}");
            }
            finally
            {
                await transaction.DisposeAsync();
                transaction = null;
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            try
            {
                return await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new InternalException(
                    InfrastructureCode.RelationalUoWCode.SaveChangesFailed,
                    $"Failed to save changes to the database: {ex.Message}");
            }
        }

        private async Task RollbackAsync()
        {
            if (transaction != null)
            {
                await transaction.RollbackAsync();
                await transaction.DisposeAsync();
                transaction = null;
            }
        }
        #endregion
    }
}