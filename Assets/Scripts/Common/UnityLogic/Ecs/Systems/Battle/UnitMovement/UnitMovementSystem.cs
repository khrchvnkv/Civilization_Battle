using System;
using System.Linq;
using Common.Infrastructure.Factories.UIFactory;
using Common.Infrastructure.Services.ECS;
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
        private IInputService _inputService;
        private IEcsStartup _ecsStartup;
        private ISceneContextService _sceneContextService;
        private IUIFactory _uiFactory;

        private EcsFilter _unitMovementFilter;
        private EcsPool<UnitMovementComponent> _unitMovementPool;

        [Inject]
        private void Construct(IInputService inputService, IEcsStartup ecsStartup, 
            ISceneContextService sceneContextService, IUIFactory uiFactory)
        {
            _inputService = inputService;
            _ecsStartup = ecsStartup;
            _sceneContextService = sceneContextService;
            _uiFactory = uiFactory;

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
                var model = component.Unit.Model;
                var path = _sceneContextService.GridMap.GetPath(model.TeamType, model.CellData, component.MoveTo.Data);
                if (path.Any()) component.ExecuteMove(path);
                _ecsStartup.World.GetPool<UnitMovementComponent>().Del(entity);
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
            ref var component = ref _ecsStartup.World.GetPool<UnitMovementComponent>().Add(unit.EntityID);
            component = new UnitMovementComponent(unit, cell);
            
            _sceneContextService.GridMap.HidePath();
        }
    }
}