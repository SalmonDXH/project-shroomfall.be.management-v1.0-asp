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
    internal class ImportItemDefinitionHandler : IHandler<ImportItemDefinitionCommand>
    {
        #region Attributes
        private readonly IUnitOfWork uow;
        private readonly ItemDefinitionService itemDefinitionService;
        #endregion

        #region Properties
        #endregion

        public ImportItemDefinitionHandler(
            IUnitOfWork uow,
            ItemDefinitionService itemDefinitionService)
        {
            this.uow = uow;
            this.itemDefinitionService = itemDefinitionService;
        }

        #region Methods
        public async Task Handle(
            ImportItemDefinitionCommand command)
        {
            // Validate json file
            if (command.File == null || command.File.Length == 0)
                throw new BadRequest(
                    ApplicationCode.DesignHandlerCode.ItemFilePayloadEmpty,
                    "The uploaded item definition file is null or empty.");

            try
            {
                // Deserialize json file
                await using var stream = command.File.OpenReadStream();
                var dtos = await JsonSerializer.DeserializeAsync<List<ItemDefinitionDTO>>(
                    stream,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                // Validate deserialized dtos
                if (dtos == null)
                    throw new BadRequest(
                        ApplicationCode.DesignHandlerCode.ItemFileSchemaParseFailed,
                        "The uploaded file does not contain a valid list of item definitions.");

                // Save changes
                await uow.BeginTransactionAsync();
                foreach (var dto in dtos)
                {
                    await itemDefinitionService.UpsertWithoutSave(dto);
                }
                await uow.CommitAsync();
            }
            catch (JsonException ex)
            {
                throw new BadRequest(
                    ApplicationCode.DesignHandlerCode.ItemFileInvalidJson,
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