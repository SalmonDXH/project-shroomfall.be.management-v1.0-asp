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
    public class LoginHandler : IHandler<LoginCommand, TokenDTO>
    {
        #region Attributes
        private readonly IUnitOfWork uow;
        private readonly TokenService tokenService;
        #endregion

        #region Properties
        #endregion

        public LoginHandler(
            IUnitOfWork uow,
            TokenService tokenService)
        {
            this.uow = uow;
            this.tokenService = tokenService;
        }

        #region Methods
        public async Task<TokenDTO> Handle(
            LoginCommand command)
        {
            var dto = command.DTO;

            // Resolve repository
            var userRepo = uow.GetRepository<IUserRepository>();

            // Validate input
            if (string.IsNullOrWhiteSpace(dto.Email))
                throw new BadRequest(
                    ApplicationCode.IdentityHandlerCode.LoginEmailRequired,
                    $"Email is required in login, login process was terminated");

            if (string.IsNullOrWhiteSpace(dto.Password))
                throw new BadRequest(
                    ApplicationCode.IdentityHandlerCode.LoginPasswordRequired,
                    $"Password is required in login, login process was terminated");

            // Validate authentication
            var email = dto.Email.Trim().ToLowerInvariant();
            var user = await userRepo.GetByEmailAsync(email);
            if (user == null)
                throw new Unauthorized(
                    ApplicationCode.IdentityHandlerCode.LoginInvalidCredentials,
                    $"Credential is invalid");
            user.VerifyPassword(dto.Password);

            // Apply domain - Login and set token
            user.UpdateLastLogin();
            (var accessToken, var refreshToken) = tokenService.Generate(user);

            // Apply persistence
            await uow.SaveChangesAsync();

            return new TokenDTO
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }
        #endregion
    }
}