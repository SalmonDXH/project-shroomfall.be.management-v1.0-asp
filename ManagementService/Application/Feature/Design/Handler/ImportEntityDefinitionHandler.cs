using Application.Feature.Abstraction;
using Application.Feature.Design.Command;
using Application.Interface.Repository.Base;
using Application.Services.DesignService;
using Contract.DTO.Definition.EntityDomain.Component;
using Domain.DomainException;
using ResponseCode;
using System.Text.Json;

namespace Application.Feature.Design.Handler
{
    internal class ImportEntityDefinitionHandler : IHandler<ImportEntityDefinitionCommand>
    {
        #region Attributes
        private readonly IUnitOfWork uow;
        private readonly EntityDefinitionService entityDefinitionService;
        #endregion

        #region Properties
        #endregion

        public ImportEntityDefinitionHandler(
            IUnitOfWork uow,
            EntityDefinitionService entityDefinitionService)
        {
            this.uow = uow;
            this.entityDefinitionService = entityDefinitionService;
        }

        #region Methods
        public async Task Handle(
            ImportEntityDefinitionCommand command)
        {
            // Validate json file
            if (command.File == null || command.File.Length == 0)
                throw new BadRequest(
                    ApplicationCode.DesignHandlerCode.EntityFilePayloadEmpty,
                    "The uploaded entity definition file is null or empty.");

            try
            {
                // Deserialize json file
                await using var stream = command.File.OpenReadStream();
                var dtos = await JsonSerializer.DeserializeAsync<List<EntityDefinitionDTO>>(
                    stream,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                // Validate deserialized dtos
                if (dtos == null)
                    throw new BadRequest(
                        ApplicationCode.DesignHandlerCode.EntityFileSchemaParseFailed,
                        "The uploaded file does not contain a valid list of entity definitions.");

                // Save changes
                await uow.BeginTransactionAsync();
                foreach (var dto in dtos)
                {
                    await entityDefinitionService.UpsertWithoutSave(dto);
                }
                await uow.CommitAsync();
            }
            catch (JsonException ex)
            {
                throw new BadRequest(
                    ApplicationCode.DesignHandlerCode.EntityFileInvalidJson,
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