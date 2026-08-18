using Contract.DTO.Feature.Identity.Command;

namespace Application.Feature.Identity.Command
{
    public class RefreshTokenCommand
    {
        #region Attributes
        #endregion

        #region Properties
        public string UserID { get; }
        public RefreshTokenDTO DTO { get; }
        #endregion

        public RefreshTokenCommand(
            string userId,
            RefreshTokenDTO dto)
        {
            UserID = userId;
            DTO = dto;
        }

        #region Methods
        #endregion
    }
}