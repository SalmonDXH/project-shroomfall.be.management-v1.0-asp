using Application.Interface.Repository;
using Application.Interface.Repository.Base;
using AutoMapper;
using Contract.DTO.Definition.EntityDomain.Component;
using Contract.DTO.Definition.LocalizationDomain;
using Contract.DTO.Definition.MetaDomain;
using Contract.DTO.Definition.WorldDomain;
using Contract.DTO.Messaging;

namespace Application.Service.DesignService
{
    public class CacheBuilder
    {
        #region Attributes
        private readonly IMapper mapper;
        private readonly IUnitOfWork uow;
        #endregion

        #region Properties
        #endregion

        public CacheBuilder(
            IMapper mapper,
            IUnitOfWork uow)
        {
            this.mapper = mapper;
            this.uow = uow;
        }

        #region Methods
        public async Task<DefinitionCacheDTO> BuildAsync(
            long version)
        {
            // Resolve all repositories on demand from the Unit of Work
            var aiRepository = uow.GetRepository<IAIDefinitionRepository>();
            var appearanceRepository = uow.GetRepository<IAppearanceDefinitionRepository>();
            var collisionRepository = uow.GetRepository<ICollisionDefinitionRepository>();
            var characteristicRepository = uow.GetRepository<ICharacteristicDefinitionRepository>();
            var inventoryRepository = uow.GetRepository<IInventoryDefinitionRepository>();
            var lifetimeRepository = uow.GetRepository<ILifetimeDefinitionRepository>();
            var projectileRepository = uow.GetRepository<IProjectileDefinitionRepository>();
            var triggeredEffectRepository = uow.GetRepository<ITriggeredEffectDefinitionRepository>();
            var entityRepository = uow.GetRepository<IEntityDefinitionRepository>();
            var localeRepository = uow.GetRepository<ILocaleRepository>();
            var effectRepository = uow.GetRepository<IEffectDefinitionRepository>();
            var itemRepository = uow.GetRepository<IItemDefinitionRepository>();
            var combatRunRepository = uow.GetRepository<ICombatRunDefinitionRepository>();
            var roomRepository = uow.GetRepository<IRoomDefinitionRepository>();

            // Prepare cache data DTO
            var roomDefinitions = await roomRepository.GetAllAsync();
            var definitionCache = new DefinitionCacheDTO
            {
                Version = version,

                Effects = mapper.Map<List<EffectDefinitionDTO>>(await effectRepository.GetAllAsync()),
                Items = mapper.Map<List<ItemDefinitionDTO>>(await itemRepository.GetAllAsync()),
                AIs = mapper.Map<List<AIDefinitionDTO>>(await aiRepository.GetAllAsync()),
                Appearances = mapper.Map<List<AppearanceDefinitionDTO>>(await appearanceRepository.GetAllAsync()),
                Collisions = mapper.Map<List<CollisionDefinitionDTO>>(await collisionRepository.GetAllAsync()),
                Characteristics = mapper.Map<List<CharacteristicDefinitionDTO>>(await characteristicRepository.GetAllAsync()),
                Inventories = mapper.Map<List<InventoryDefinitionDTO>>(await inventoryRepository.GetAllAsync()),
                Lifetimes = mapper.Map<List<LifetimeDefinitionDTO>>(await lifetimeRepository.GetAllAsync()),
                Projectiles = mapper.Map<List<ProjectileDefinitionDTO>>(await projectileRepository.GetAllAsync()),
                TriggeredEffects = mapper.Map<List<TriggeredEffectDefinitionDTO>>(await triggeredEffectRepository.GetAllAsync()),
                Entities = mapper.Map<List<EntityDefinitionDTO>>(await entityRepository.GetAllAsync()),
                CombatRuns = mapper.Map<List<CombatRunDefinitionDTO>>(await combatRunRepository.GetAllAsync()),
                Rooms = mapper.Map<List<RoomDefinitionDTO>>(roomDefinitions),
                EntitySpawnRules = mapper.Map<List<EntitySpawnRuleDTO>>(roomDefinitions.SelectMany(room => room.EntitySpawnRules)),
                Cells = mapper.Map<List<CellDTO>>(roomDefinitions.SelectMany(room => room.Cells)),
                Locales = mapper.Map<List<LocaleDTO>>(await localeRepository.GetAllAsync())
            };

            return definitionCache;
        }
        #endregion
    }
}