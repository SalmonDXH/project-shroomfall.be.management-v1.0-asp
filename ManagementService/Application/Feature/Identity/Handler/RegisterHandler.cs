using Application.Feature.Abstraction;
using Application.Feature.Identity.Command;
using Application.Interface.Repository;
using Application.Interface.Repository.Base;
using Application.Service.IdentityService;
using Contract.DTO.Feature.Identity.Response;
using Contract.Enum.IdentityDomain;
using Domain.DomainException;
using Domain.IdentityDomain;
using ResponseCode;
using System.Data;

namespace Application.Feature.Identity.Handler
{
    public class RegisterHandler : IHandler<RegisterCommand, TokenDTO>
    {
        #region Attributes
        private readonly IUnitOfWork uow;
        private readonly TokenService tokenService;
        #endregion

        #region Properties
        #endregion

        public RegisterHandler(
            IUnitOfWork uow,
            TokenService tokenService)
        {
            this.uow = uow;
            this.tokenService = tokenService;
        }

        #region Methods
        public async Task<TokenDTO> Handle(
            RegisterCommand command)
        {
            var dto = command.DTO;

            // Resolve repositories
            var userRepo = uow.GetRepository<IUserRepository>();

            // Validate fields
            if (string.IsNullOrWhiteSpace(dto.Email))
                throw new BadRequest(
                    ApplicationCode.IdentityHandlerCode.RegisterEmailRequired,
                    $"Email is required in registration, registration process was terminated");

            // Validate email existence
            var email = dto.Email.Trim().ToLowerInvariant();
            var existed = await userRepo.GetByEmailAsync(email);
            if (existed != null)
                throw new BadRequest(
                    ApplicationCode.IdentityHandlerCode.RegisterEmailAlreadyExists,
                    $"Email already existed when registered {dto.Email}");

            // Apply domain - Create user
            var user = new User(
                id: Guid.NewGuid().ToString(),
                name: dto.Name ?? "Player",
                role: Role.Player,
                password: Password.Create(dto.Password),
                email: email
            );

            // Apply domain - Login and set token
            user.UpdateLastLogin();
            (var accessToken, var refreshToken) = tokenService.Generate(user);

            // Apply persistence
            await uow.BeginTransactionAsync();
            await userRepo.AddAsync(user);
            await uow.CommitAsync();

            return new TokenDTO
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }
        #endregion
    }
}