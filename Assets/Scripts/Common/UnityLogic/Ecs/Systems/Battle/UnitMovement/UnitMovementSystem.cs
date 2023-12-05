using System;
using Common.Infrastructure.Factories.UIFactory;
using Common.Infrastructure.Factories.UnitsFactory;
using Common.Infrastructure.Services.Input;
using Common.Infrastructure.Services.SceneContext;
using Common.UnityLogic.Builders.Grid;
using Common.UnityLogic.Ecs.Components.Units;
using Common.UnityLogic.UI.Windows.GameHUD;
using Common.UnityLogic.Units;
using Leopotam.EcsLite;
using Zenject;

namespace Common.UnityLogic.Ecs.Systems.Battle.UnitMovement
{
    /// <summary>
    /// Перемещение юнита
    /// </summary>
    public sealed class UnitMovementSystem : IEcsInitSystem, IEcsRunSystem, IDisposable
    {
        private IUnitsControlService _unitsControlService;
        private ISceneContextService _sceneContextService;
        private IUIFactory _uiFactory;

        private EcsFilter _unitMovementFilter;
        private EcsPool<UnitMovementComponent> _unitMovementPool;
        private EcsPool<UnitTeamComponent> _unitTeamPool;

        [Inject]
        private void Construct(IUnitsControlService unitsControlService, ISceneContextService sceneContextService, IUIFactory uiFactory)
        {
            _unitsControlService = unitsControlService;
            _sceneContextService = sceneContextService;
            _uiFactory = uiFactory;

            _unitsControlService.UnitMoveClicked += UnitMoveClicked;
        }

        public void Dispose() => _unitsControlService.UnitMoveClicked -= UnitMoveClicked;
        
        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            _unitMovementFilter = world.Filter<UnitMovementComponent>().End();
            _unitMovementPool = world.GetPool<UnitMovementComponent>();
            _unitTeamPool = world.GetPool<UnitTeamComponent>();
        }
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _unitMovementFilter)
            {
                ref var component = ref _unitMovementPool.Get(entity);
                var model = component.Unit.Model;
                var path = _sceneContextService.GridMap.GetPath(model.TeamType, model.CellData, component.MoveTo.Data);
                var range = path.Count;
                _sceneContextService.GridMap.IsEnemyLocated(model.TeamType, component.MoveTo.Data, out var attackedUnit);
                
                component.Unit.MoveUnit(path, attackedUnit);

                ref var teamComponent = ref _unitTeamPool.Get(component.Unit.EntityID);
                teamComponent.UnitModel.AvailableMovementRange -= range;

                // Auto select next unit
                if (!teamComponent.UnitModel.HasAvailableRange) _unitsControlService.SelectNextAvailableUnit();

                _unitMovementPool.Del(entity);
            }
        }

        private void UnitMoveClicked(Unit unit, Cell cell)
        {
            _uiFactory.ShowWindow(new GameHudWindowData());
            
            if (unit.Model.CellData == cell.Data || !_sceneContextService.GridMap.CellAvailable(cell.Data))
            {
                _sceneContextService.GridMap.HidePath();
                return;
            }
            
            // Move unit
            ref var component = ref _unitMovementPool.Add(unit.EntityID);
            component = new UnitMovementComponent(unit, cell);
            
            _sceneContextService.GridMap.HidePath();
        }
    }
}