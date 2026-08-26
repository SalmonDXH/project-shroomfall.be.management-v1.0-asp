using Microsoft.AspNetCore.Http;

namespace Application.Feature.Design.Command
{
    public class ImportEntityDefinitionCommand
    {
        #region Attributes
        #endregion

        #region Properties
        public IFormFile File { get; }
        #endregion

        public ImportEntityDefinitionCommand(
            IFormFile file)
        {
            File = file;
        }

        #region Methods
        #endregion
    }
}