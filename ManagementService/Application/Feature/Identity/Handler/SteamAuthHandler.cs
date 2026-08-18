using Application.Feature.Abstraction;
using Application.Feature.Identity.Command;
using Application.Interface.Repository;
using Application.Interface.Repository.Base;
using Application.Interface.Utility;
using Application.Service.IdentityService;
using Contract.DTO.Feature.Identity.Response;
using Contract.Enum.IdentityDomain;
using Domain.DomainException;
using Domain.IdentityDomain;
using ResponseCode;
using System.Data;

namespace Application.Feature.Identity.Handler
{
    public class SteamAuthHandler : IHandler<SteamAuthCommand, TokenDTO>
    {
        #region Attributes
        private readonly IUnitOfWork uow;
        private readonly ISteamValidator steamValidator;
        private readonly TokenService tokenService;
        #endregion

        #region Properties
        #endregion

        public SteamAuthHandler(
            IUnitOfWork uow,
            ISteamValidator steamValidator,
            TokenService tokenService)
        {
            this.uow = uow;
            this.steamValidator = steamValidator;
            this.tokenService = tokenService;
        }

        #region Methods
        public async Task<TokenDTO> Handle(
            SteamAuthCommand command)
        {
            var dto = command.DTO;

            // Resolve repository
            var userRepo = uow.GetRepository<IUserRepository>();

            // Validate steam ticket
            if (string.IsNullOrEmpty(dto.SteamTicket))
                throw new BadRequest(
                    ApplicationCode.IdentityHandlerCode.SteamAuthInvalidSteamTicket,
                    $"Steam ticket is invalid, can not authenticate by steam");

            // Validate steam ID
            var steamId = await steamValidator.ValidateTicket(dto.SteamTicket);
            if (string.IsNullOrEmpty(steamId))
                throw new Unauthorized(
                    ApplicationCode.IdentityHandlerCode.SteamAuthValidationFailed,
                    $"Steam validation was failed, there no such steam ID found");

            // Check existence
            var user = await userRepo.GetBySteamIdAsync(steamId);

            // Steam authentication logic
            string accessToken;
            string refreshToken;
            if (user == null)
            {
                // Apply domain - Create user
                user = new User(
                    id: Guid.NewGuid().ToString(),
                    name: dto.SteamName ?? "Player",
                    role: Role.Player,
                    steamId: steamId
                );

                // Apply domain - Login
                user.UpdateLastLogin();
                (accessToken, refreshToken) = tokenService.Generate(user);

                // Apply persistence
                await uow.BeginTransactionAsync();
                await userRepo.AddAsync(user);
                await uow.CommitAsync();
            }
            else
            {
                // Apply domain - Login
                user.UpdateLastLogin();
                (accessToken, refreshToken) = tokenService.Generate(user);

                // Apply persistence
                await uow.SaveChangesAsync();
            }

            return new TokenDTO
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }
        #endregion
    }
}