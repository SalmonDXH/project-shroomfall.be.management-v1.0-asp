using Microsoft.AspNetCore.Http;

namespace Application.Feature.Design.Command
{
    public class ImportLocalizationEntryCommand
    {
        #region Attributes
        #endregion

        #region Properties
        public string LocaleCode { get; }
        public IFormFile File { get; }
        #endregion

        public ImportLocalizationEntryCommand(
            string localeCode,
            IFormFile file)
        {
            LocaleCode = localeCode;
            File = file;
        }

        #region Methods
        #endregion
    }
}