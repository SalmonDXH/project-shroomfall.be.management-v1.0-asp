using Application.Feature.Abstraction;
using Application.Feature.Design.Command;
using Application.Interface.Repository;
using Application.Interface.Repository.Base;
using Domain.DomainException;
using ResponseCode;
using System.Text.Json;

namespace Application.Feature.Design.Handler
{
    internal class ImportLocalizationEntryHandler : IHandler<ImportLocalizationEntryCommand>
    {
        #region Attributes
        private readonly IUnitOfWork uow;
        #endregion

        #region Properties
        #endregion

        public ImportLocalizationEntryHandler(
            IUnitOfWork uow)
        {
            this.uow = uow;
        }

        #region Methods
        public async Task Handle(
            ImportLocalizationEntryCommand command)
        {
            // Validate json file
            if (command.File == null || command.File.Length == 0)
                throw new BadRequest(
                    ApplicationCode.DesignHandlerCode.LocalizationFilePayloadEmpty,
                    "The uploaded localization file is null or empty.");

            Dictionary<string, string>? values;
            try
            {
                // Deserialize json file: { "key": "localized value", ... }
                await using var stream = command.File.OpenReadStream();
                values = await JsonSerializer.DeserializeAsync<Dictionary<string, string>>(stream);
            }
            catch (JsonException ex)
            {
                throw new BadRequest(
                    ApplicationCode.DesignHandlerCode.LocalizationFileInvalidJson,
                    $"Failed to deserialize JSON stream due to formatting errors: {ex.Message}");
            }

            if (values == null)
                throw new BadRequest(
                    ApplicationCode.DesignHandlerCode.LocalizationFileSchemaParseFailed,
                    "The uploaded file does not contain a valid key/value localization map.");

            // Only existing keys of this locale are updated, unknown keys are ignored
            var localeRepo = uow.GetRepository<ILocaleRepository>();
            var entries = await localeRepo.GetByKeysAsync(command.LocaleCode, values.Keys);
            var trimmedValues = new Dictionary<string, string>();
            foreach (var (key, value) in values)
                trimmedValues[key.Trim()] = value; // last one wins if keys collide after trimming

            foreach (var entry in entries)
            {
                var value = trimmedValues[entry.Key];
                if (entry.Value != value)
                    entry.Update(value, entry.Description);
            }

            await uow.SaveChangesAsync();
        }
        #endregion
    }
}