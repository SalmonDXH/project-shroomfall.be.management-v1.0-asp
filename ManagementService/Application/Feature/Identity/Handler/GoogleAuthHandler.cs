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

namespace Application.Feature.Identity.Handler
{
    public class GoogleAuthHandler : IHandler<GoogleAuthCommand, TokenDTO>
    {
        #region Attributes
        private readonly IUnitOfWork uow;
        private readonly IGoogleTokenValidator googleTokenValidator;
        private readonly TokenService tokenService;
        #endregion

        #region Properties
        #endregion

        public GoogleAuthHandler(
            IUnitOfWork uow,
            IGoogleTokenValidator googleTokenValidator,
            TokenService tokenService)
        {
            this.uow = uow;
            this.googleTokenValidator = googleTokenValidator;
            this.tokenService = tokenService;
        }

        #region Methods
        public async Task<TokenDTO> Handle(
            GoogleAuthCommand command)
        {
            var dto = command.DTO;

            // Resolve repository
            var userRepo = uow.GetRepository<IUserRepository>();

            // Validate google id token
            if (string.IsNullOrEmpty(dto.IdToken))
                throw new BadRequest(
                    ApplicationCode.IdentityHandlerCode.GoogleIdTokenMissing,
                    $"Google ID token is invalid, can not authenticate by google");

            // Validate Google ID token
            var payload = await googleTokenValidator.ValidateTokenAsync(dto.IdToken);
            if (payload == null || string.IsNullOrEmpty(payload.Subject))
                throw new Unauthorized(
                    ApplicationCode.IdentityHandlerCode.GoogleAuthPayloadMissing,
                    $"Google token payload is invalid or missing subject identifier.");

            // Extract the unique Google User ID (Subject)
            string googleId = payload.Subject;

            // Check existence
            var user = await userRepo.GetByGoogleIdAsync(googleId);

            // Google authentication logic
            TokenDTO token = new TokenDTO();
            if (user == null)
            {
                // Apply domain - Create user
                user = new User(
                    id: Guid.NewGuid().ToString(),
                    name: payload.Name ?? "Player",
                    role: Role.Player,
                    googleId: googleId
                );

                // Apply domain - Login
                user.UpdateLastLogin();
                token = tokenService.Generate(user);

                // Apply persistence
                await uow.BeginTransactionAsync();
                await userRepo.AddAsync(user);
                await uow.CommitAsync();
            }
            else
            {
                // Apply domain - Login
                user.UpdateLastLogin();
                token = tokenService.Generate(user);

                // Apply persistence
                await uow.SaveChangesAsync();
            }

            return token;
        }
        #endregion
    }
}