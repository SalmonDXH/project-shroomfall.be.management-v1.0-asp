using Application.Feature.Abstraction;
using Application.Interface.Utility;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Feature
{
    public class Dispatcher : IDispatcher
    {
        #region Attributes
        private readonly IServiceProvider serviceProvider;
        #endregion

        #region Properties
        #endregion

        public Dispatcher(
            IServiceProvider serviceProvider
        )
        {
            this.serviceProvider = serviceProvider;
        }

        #region Methods
        public async Task<TResponse> Send<TCommand, TResponse>(
            TCommand command)
        {
            var handler = serviceProvider.GetRequiredService<IHandler<TCommand, TResponse>>();
            return await handler.Handle(command);
        }

        public async Task Send<TCommand>(
            TCommand command)
        {
            var handler = serviceProvider.GetRequiredService<IHandler<TCommand>>();
            await handler.Handle(command);
        }
        #endregion
    }
}