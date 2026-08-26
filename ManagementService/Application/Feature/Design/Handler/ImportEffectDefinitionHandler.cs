using Application.Feature.Abstraction;
using Application.Feature.Design.Command;
using Application.Interface.Repository.Base;
using Application.Service.DesignService;
using Contract.DTO.Definition.MetaDomain;
using Domain.DomainException;
using ResponseCode;
using System.Text.Json;

namespace Application.Feature.Design.Handler
{
    internal class ImportEffectDefinitionHandler : IHandler<ImportEffectDefinitionCommand>
    {
        #region Attributes
        private readonly IUnitOfWork uow;
        private readonly EffectDefinitionService effectDefinitionService;
        #endregion

        #region Properties
        #endregion

        public ImportEffectDefinitionHandler(
            IUnitOfWork uow,
            EffectDefinitionService effectDefinitionService)
        {
            this.uow = uow;
            this.effectDefinitionService = effectDefinitionService;
        }

        #region Methods
        public async Task Handle(
            ImportEffectDefinitionCommand command)
        {
            // Validate json file
            if (command.File == null || command.File.Length == 0)
                throw new BadRequest(
                    ApplicationCode.DesignHandlerCode.EffectFilePayloadEmpty,
                    "The uploaded effect definition file is null or empty.");

            try
            {
                // Deserialize json file
                await using var stream = command.File.OpenReadStream();
                var dtos = await JsonSerializer.DeserializeAsync<List<EffectDefinitionDTO>>(
                    stream,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                // Validate deserialized dtos
                if (dtos == null)
                    throw new BadRequest(
                        ApplicationCode.DesignHandlerCode.EffectFileSchemaParseFailed,
                        "The uploaded file does not contain a valid list of effect definitions.");

                // Save changes
                await uow.BeginTransactionAsync();
                foreach (var dto in dtos) { await effectDefinitionService.UpsertWithoutSave(dto); }
                await uow.CommitAsync();
            }
            catch (JsonException ex)
            {
                throw new BadRequest(
                    ApplicationCode.DesignHandlerCode.EffectFileInvalidJson,
                    $"Failed to deserialize JSON stream due to formatting errors: {ex.Message}");
            }
            catch
            {
                throw;
            }
        }
        #endregion
    }
}