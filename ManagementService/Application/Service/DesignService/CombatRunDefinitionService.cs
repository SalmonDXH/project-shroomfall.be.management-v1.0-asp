using Application.Interface.Repository;
using Application.Interface.Repository.Base;
using Contract.DTO.Definition.WorldDomain;
using Domain.WorldDomain;

namespace Application.Service.DesignService
{
    public class CombatRunDefinitionService
    {
        #region Attributes
        private readonly IUnitOfWork uow;
        #endregion

        #region Properties
        #endregion

        public CombatRunDefinitionService(
            IUnitOfWork uow)
        {
            this.uow = uow;
        }

        #region Methods
        public async Task UpsertWithoutSave(
            CombatRunDefinitionDTO dto)
        {
            // Upsert flow
            var combatRunRepo = uow.GetRepository<ICombatRunDefinitionRepository>();
            var existingCombatRun = await combatRunRepo.GetByIdAsync(dto.Id);
            if (existingCombatRun == null)
            {
                // CREATE FLOW 
                var combatRun = new CombatRunDefinition(dto.Id);
                await combatRunRepo.AddAsync(combatRun);
            }

            // ALL FLOWS
            var floors = dto.Floors.Select(f => new Floor(f.Level, f.RoomDefinitionID, dto.Id));
            await combatRunRepo.UpsertFloorsAsync(dto.Id, floors);
        }
        #endregion
    }
}