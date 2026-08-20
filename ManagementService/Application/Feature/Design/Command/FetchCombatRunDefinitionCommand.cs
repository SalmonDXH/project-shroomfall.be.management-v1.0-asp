using Contract.DTO.Feature.Design.Command;

namespace Application.Feature.Design.Command
{
    public class FetchCombatRunDefinitionCommand
    {
        #region Attributes
        #endregion

        #region Properties
        public string UserID { get; }
        public CombatRunDefinitionQueryDTO Queries { get; }
        #endregion

        public FetchCombatRunDefinitionCommand(
            string userId,
            CombatRunDefinitionQueryDTO queries)
        {
            Queries = queries;
            UserID = userId;
        }

        #region Methods
        #endregion
    }
}