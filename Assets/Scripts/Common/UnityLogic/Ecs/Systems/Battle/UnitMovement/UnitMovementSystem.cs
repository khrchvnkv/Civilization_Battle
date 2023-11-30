using System;
using Common.Infrastructure.Services.ECS;
using Common.Infrastructure.Services.Input;
using Common.Infrastructure.Services.SceneContext;
using Common.UnityLogic.Builders.Grid;
using Common.UnityLogic.Ecs.Components.Units;
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
        private IInputService _inputService;
        private IEcsStartup _ecsStartup;
        private ISceneContextService _sceneContextService;

        private EcsFilter _unitMovementFilter;
        private EcsPool<UnitMovementComponent> _unitMovementPool;

        [Inject]
        private void Construct(IInputService inputService, IEcsStartup ecsStartup, ISceneContextService sceneContextService)
        {
            _inputService = inputService;
            _ecsStartup = ecsStartup;
            _sceneContextService = sceneContextService;

            _inputService.UnitMoveClicked += UnitMoveClicked;
        }

        public void Dispose() => _inputService.UnitMoveClicked -= UnitMoveClicked;
        
        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            _unitMovementFilter = world.Filter<UnitMovementComponent>().End();
            _unitMovementPool = world.GetPool<UnitMovementComponent>();
        }
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _unitMovementFilter)
            {
                ref var component = ref _ecsStartup.World.GetPool<UnitMovementComponent>().Get(entity);
                var path = _sceneContextService.GridMap.GetPath(component.Unit.CellData, component.MoveTo.Data);
                component.ExecuteMove(path);
                _ecsStartup.World.GetPool<UnitMovementComponent>().Del(entity);
            }
        }

        private void UnitMoveClicked(Unit unit, Cell cell)
        {
            if (unit.CellData == cell.Data) return;
            if (!_sceneContextService.GridMap.CellAvailable(cell.Data)) return;
            
            // Move unit
            ref var component = ref _ecsStartup.World.GetPool<UnitMovementComponent>().Add(unit.EntityID);
            component = new UnitMovementComponent(unit, cell);
            
            _sceneContextService.GridMap.HidePath();
        }
    }
}