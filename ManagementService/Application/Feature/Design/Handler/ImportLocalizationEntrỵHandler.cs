using Application.Feature.Abstraction;
using Application.Feature.Design.Command;
using Application.Interface.Repository;
using Application.Interface.Repository.Base;
using Contract.DTO.Definition.LocalizationDomain;
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

            List<LocaleDTO>? locales;
            try
            {
                // Deserialize json file: [ { "Code": "en-US", "LocalizationEntries": [ { "Key", "Value" } ] } ]
                await using var stream = command.File.OpenReadStream();
                locales = await JsonSerializer.DeserializeAsync<List<LocaleDTO>>(
                    stream,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (JsonException ex)
            {
                throw new BadRequest(
                    ApplicationCode.DesignHandlerCode.LocalizationFileInvalidJson,
                    $"Failed to deserialize JSON stream due to formatting errors: {ex.Message}");
            }

            if (locales == null)
                throw new BadRequest(
                    ApplicationCode.DesignHandlerCode.LocalizationFileSchemaParseFailed,
                    "The uploaded file does not contain a valid list of locales.");

            // Take only the locale block matching the url locale
            var locale = locales.FirstOrDefault(l =>
                string.Equals(l.Code.Trim(), command.LocaleCode.Trim(), StringComparison.OrdinalIgnoreCase));
            if (locale == null || locale.LocalizationEntries == null || locale.LocalizationEntries.Count == 0)
                return;

            var values = new Dictionary<string, string>();
            foreach (var dto in locale.LocalizationEntries)
                if (!string.IsNullOrWhiteSpace(dto.Key))
                    values[dto.Key.Trim()] = dto.Value; // last one wins on duplicate keys

            // Only existing keys of this locale are updated, unknown keys are ignored
            var localeRepo = uow.GetRepository<ILocaleRepository>();
            var entries = await localeRepo.GetByKeysAsync(command.LocaleCode, values.Keys);

            foreach (var entry in entries)
            {
                var value = values[entry.Key];
                if (entry.Value != value)
                    entry.Update(value, entry.Description);
            }

            await uow.SaveChangesAsync();
        }
        #endregion
    }
}