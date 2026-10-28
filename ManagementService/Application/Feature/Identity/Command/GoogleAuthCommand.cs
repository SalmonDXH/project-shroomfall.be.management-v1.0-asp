using Contract.DTO.Feature.Identity.Command;

namespace Application.Feature.Identity.Command
{
    public class GoogleAuthCommand
    {
        #region Attributes
        #endregion

        #region Properties
        public GoogleAuthDTO DTO { get; }
        #endregion

        public GoogleAuthCommand(
            GoogleAuthDTO dto)
        {
            DTO = dto;
        }

        #region Methods
        #endregion
    }
}