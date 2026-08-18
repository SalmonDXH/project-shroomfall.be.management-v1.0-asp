using Application.Feature.Abstraction;
using Application.Feature.Identity.Command;
using Application.Interface.Repository;
using Application.Interface.Repository.Base;
using Domain.DomainException;
using ResponseCode;

namespace Application.Feature.Identity.Handler
{
    public class UpdateProfileHandler : IHandler<UpdateProfileCommand>
    {
        #region Attributes
        private readonly IUnitOfWork uow;
        #endregion

        #region Properties
        #endregion

        public UpdateProfileHandler(
            IUnitOfWork uow)
        {
            this.uow = uow;
        }

        #region Methods
        public async Task Handle(
            UpdateProfileCommand command)
        {
            var dto = command.DTO;

            // Resolve repository
            var userRepo = uow.GetRepository<IUserRepository>();

            // Validate existence
            var user = await userRepo.GetByIdAsync(command.UserID);
            if (user == null)
                throw new NotFound(
                    ApplicationCode.IdentityHandlerCode.UpdateProfileUserNotFound,
                    $"User with user ID: {command.UserID} was not found");

            // Apply domain - Update profile
            user.UpdateProfile(
                dto.Name,
                dto.Dob,
                dto.Gender
            );

            // Apply persistence
            await uow.SaveChangesAsync();
        }
        #endregion
    }
}