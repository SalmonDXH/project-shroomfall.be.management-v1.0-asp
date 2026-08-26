using Microsoft.AspNetCore.Http;

namespace Application.Feature.Design.Command
{
    public class ImportRoomDefinitionCommand
    {
        #region Attributes
        #endregion

        #region Properties
        public IFormFile File { get; }
        #endregion

        public ImportRoomDefinitionCommand(
            IFormFile file)
        {
            File = file;
        }

        #region Methods
        #endregion
    }
}