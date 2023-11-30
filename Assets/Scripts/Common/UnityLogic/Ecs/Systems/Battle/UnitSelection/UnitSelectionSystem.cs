using System;
using Common.Infrastructure.Services.ECS;
using Common.Infrastructure.Services.Input;
using Common.Infrastructure.Services.SceneContext;
using Common.UnityLogic.Ecs.Components.Units;
using Common.UnityLogic.Ecs.OneFrames;
using Common.UnityLogic.Units;
using Leopotam.EcsLite;
using Zenject;

namespace Common.UnityLogic.Ecs.Systems.Battle.UnitSelection
{
    /// <summary>
    /// Выбор юнита
    /// </summary>
    public sealed class UnitSelectionSystem : IEcsInitSystem, IEcsRunSystem, IDisposable
    {
        private IInputService _inputService;
        private IEcsStartup _ecsStartup;
        private ISceneContextService _sceneContextService;

        private EcsFilter _selectionUnitFilter;
        private EcsPool<SelectUnitEvent> _selectionUnitPool;

        [Inject]
        private void Construct(IInputService inputService, IEcsStartup ecsStartup, ISceneContextService sceneContextService)
        {
            _inputService = inputService;
            _ecsStartup = ecsStartup;
            _sceneContextService = sceneContextService;

            _inputService.UnitClicked += UnitSelected;
        }
        
        public void Dispose() => _inputService.UnitClicked -= UnitSelected;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            _selectionUnitFilter = world.Filter<SelectUnitEvent>().End();
            _selectionUnitPool = world.GetPool<SelectUnitEvent>();
        }
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _selectionUnitFilter)
            {
                ref var component = ref _selectionUnitPool.Get(entity);
                _sceneContextService.GridMap.ShowPath(component.Unit.CellData, component.Unit.StaticData.Range);
                _selectionUnitPool.Del(entity);
            }
        }
        
        private void UnitSelected(Unit unit)
        {
            _sceneContextService.GridMap.HidePath();

            var entity = unit.EntityID;
            ref var teamComponent = ref _ecsStartup.World.GetPool<UnitTeamComponent>().Get(entity);
            
            if (!teamComponent.IsActiveTeam) return;
            
            ref var selectComponent = ref _ecsStartup.World.GetPool<SelectUnitEvent>().Add(entity);
            selectComponent = new SelectUnitEvent(unit);
        }
    }
}