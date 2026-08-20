using API.Helper;
using Application.Feature.Abstraction;
using Application.Feature.Design.Command;
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
        #endregion
    }
}