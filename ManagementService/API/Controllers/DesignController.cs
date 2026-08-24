using API.Helper;
using Application.Feature.Abstraction;
using Application.Feature.Design.Command;
using Application.Features.Design.Command;
using Contract.DTO.Abstraction;
using Contract.DTO.Definition.EntityDomain.Component;
using Contract.DTO.Definition.LocalizationDomain;
using Contract.DTO.Definition.MetaDomain;
using Contract.DTO.Definition.WorldDomain;
using Contract.DTO.Feature.Design.Command;
using Contract.DTO.Feature.Design.Response;
using Contract.Enum.IdentityDomain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DesignController : ControllerBase
    {
        #region Attributes
        private readonly IDispatcher dispatcher;
        #endregion

        #region Properties
        #endregion

        public DesignController(
            IDispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
        }

        #region Methods
        [Authorize(Roles = nameof(Role.Designer) + "," + nameof(Role.Admin))]
        [HttpGet("combat-run")]
        public async Task<ActionResult<PagedResponseDTO<CombatRunDefinitionDTO>>> GetAllCombatRuns(
            [FromQuery] CombatRunDefinitionQueryDTO queries)
        {
            var (userId, steamId, role) = ClaimReader.GetIdentity(User);

            var result = await dispatcher.Send<FetchCombatRunDefinitionCommand, PagedResponseDTO<CombatRunDefinitionDTO>>(
                new FetchCombatRunDefinitionCommand(userId, queries)
            );

            return Ok(result);
        }

        [Authorize(Roles = nameof(Role.Designer) + "," + nameof(Role.Admin))]
        [HttpGet("effects")]
        public async Task<ActionResult<PagedResponseDTO<EffectDefinitionDTO>>> GetAllEffects(
            [FromQuery] EffectDefinitionQueryDTO queries)
        {
            var (userId, steamId, role) = ClaimReader.GetIdentity(User);

            var result = await dispatcher.Send<FetchEffectDefinitionCommand, PagedResponseDTO<EffectDefinitionDTO>>(
                new FetchEffectDefinitionCommand(userId, queries)
            );

            return Ok(result);
        }

        [Authorize(Roles = nameof(Role.Designer) + "," + nameof(Role.Admin))]
        [HttpGet("entities")]
        public async Task<ActionResult<PagedResponseDTO<EntityDefinitionDTO>>> GetAllEntities(
            [FromQuery] EntityDefinitionQueryDTO queries)
        {
            var (userId, steamId, role) = ClaimReader.GetIdentity(User);

            var result = await dispatcher.Send<FetchEntityDefinitionCommand, PagedResponseDTO<EntityDefinitionDTO>>(
                new FetchEntityDefinitionCommand(userId, queries)
            );

            return Ok(result);
        }

        [Authorize(Roles = nameof(Role.Designer) + "," + nameof(Role.Admin))]
        [HttpGet("entities/{id}")]
        public async Task<ActionResult<EffectDefinitionDTO>> GetEntityDefinitionDetail(
            [FromRoute] string id)
        {
            var (userId, steamId, role) = ClaimReader.GetIdentity(User);

            var result = await dispatcher.Send<FetchEntityDefinitionDetailCommand, EntityDefinitionDTO?>(
                new FetchEntityDefinitionDetailCommand(userId, id)
            );

            return Ok(result);
        }

        [Authorize(Roles = nameof(Role.Designer) + "," + nameof(Role.Admin))]
        [HttpGet("items")]
        public async Task<ActionResult<PagedResponseDTO<ItemDefinitionDTO>>> GetAllItems(
            [FromQuery] ItemDefinitionQueryDTO queries)
        {
            var (userId, steamId, role) = ClaimReader.GetIdentity(User);

            var result = await dispatcher.Send<FetchItemDefinitionCommand, PagedResponseDTO<ItemDefinitionDTO>>(
                new FetchItemDefinitionCommand(userId, queries)
            );

            return Ok(result);
        }

        [Authorize(Roles = nameof(Role.Designer) + "," + nameof(Role.Admin))]
        [HttpGet("locales")]
        public async Task<ActionResult<List<LocaleDTO>>> GetAllLocales()
        {
            var (userId, steamId, role) = ClaimReader.GetIdentity(User);

            var result = await dispatcher.Send<FetchLocaleCommand, List<LocaleDTO>>(
                new FetchLocaleCommand(userId)
            );

            return Ok(result);
        }

        [Authorize(Roles = nameof(Role.Designer) + "," + nameof(Role.Admin))]
        [HttpGet("localization-entries")]
        public async Task<ActionResult<PagedResponseDTO<LocalizationEntryDTO>>> GetLocalizationEntries(
            [FromQuery] LocalizationEntryQueryDTO queries)
        {
            var (userId, steamId, role) = ClaimReader.GetIdentity(User);

            var result = await dispatcher.Send<FetchLocalizationEntryCommand, PagedResponseDTO<LocalizationEntryDTO>>(
                new FetchLocalizationEntryCommand(userId, queries)
            );

            return Ok(result);
        }

        [Authorize(Roles = nameof(Role.Designer) + "," + nameof(Role.Admin))]
        [HttpGet("rooms")]
        public async Task<ActionResult<PagedResponseDTO<RoomDefinitionDTO>>> GetAllRooms(
            [FromQuery] RoomDefinitionQueryDTO queries)
        {
            var (userId, steamId, role) = ClaimReader.GetIdentity(User);

            var result = await dispatcher.Send<FetchRoomDefinitionCommand, PagedResponseDTO<RoomDefinitionDTO>>(
                new FetchRoomDefinitionCommand(userId, queries)
            );

            return Ok(result);
        }
        #endregion
    }
}