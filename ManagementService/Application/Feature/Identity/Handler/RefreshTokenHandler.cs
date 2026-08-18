using Application.Feature.Abstraction;
using Application.Feature.Identity.Command;
using Application.Interface.Repository;
using Application.Interface.Repository.Base;
using Application.Service.IdentityService;
using Contract.DTO.Feature.Identity.Response;
using Domain.DomainException;
using ResponseCode;

namespace Application.Feature.Identity.Handler
{
    public class RefreshTokenHandler : IHandler<RefreshTokenCommand, TokenDTO>
    {
        #region Attributes
        private readonly IUnitOfWork uow;
        private readonly TokenService tokenService;
        #endregion

        #region Properties
        #endregion

        public RefreshTokenHandler(
            IUnitOfWork uow,
            TokenService tokenService)
        {
            this.uow = uow;
            this.tokenService = tokenService;
        }

        #region Methods
        public async Task<TokenDTO> Handle(
            RefreshTokenCommand command)
        {
            var dto = command.DTO;

            // Resolve repository
            var userRepo = uow.GetRepository<IUserRepository>();

            // Validate authentication
            var user = await userRepo.GetByIdAsync(command.UserID);
            if (user == null)
                throw new NotFound(
                    ApplicationCode.IdentityHandlerCode.RefreshTokenUserNotFound,
                    $"User with user ID: {command.UserID} was not found");

            // Validate refresh token 
            user.ValidateRefreshToken(dto.RefreshToken, DateTime.UtcNow);

            // Apply domain - Set token
            (var accessToken, var newRefreshToken) = tokenService.Generate(user);

            // Apply persistence
            await uow.SaveChangesAsync();

            return new TokenDTO
            {
                AccessToken = accessToken,
                RefreshToken = newRefreshToken
            };
        }
        #endregion
    }
}