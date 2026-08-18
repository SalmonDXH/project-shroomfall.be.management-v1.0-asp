using Contract.DTO.Feature.Identity.Command;

namespace Application.Feature.Identity.Command
{
    public class RegisterCommand
    {
        #region Attributes
        #endregion

        #region Properties
        public RegisterDTO DTO { get; }
        #endregion

        public RegisterCommand(
            RegisterDTO dTO)
        {
            DTO = dTO;
        }

        #region Methods
        #endregion
    }
}