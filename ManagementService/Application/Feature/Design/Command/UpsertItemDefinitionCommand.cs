using Contract.DTO.Definition.MetaDomain;

namespace Application.Feature.Design.Command
{
    public class UpsertItemDefinitionCommand
    {
        #region Attributes
        #endregion

        #region Properties
        public string UserID { get; }
        public ItemDefinitionDTO DTO { get; }
        #endregion

        public UpsertItemDefinitionCommand(
            string userId,
            ItemDefinitionDTO dto)
        {
            UserID = userId;
            DTO = dto;
        }

        #region Methods
        #endregion
    }
}