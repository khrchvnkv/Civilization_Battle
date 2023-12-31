using System;
using Common.Infrastructure.Factories.UIFactory;
using Common.Infrastructure.Services.Input;
using Common.Infrastructure.Services.SceneContext;
using Common.UnityLogic.Ecs.Components.Units;
using Common.UnityLogic.Ecs.OneFrames;
using Common.UnityLogic.UI.Windows.GameHUD;
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
        private IUnitsControlService _unitsControlService;
        private ISceneContextService _sceneContextService;
        private IUIFactory _uiFactory;

        private EcsFilter _selectionUnitFilter;
        private EcsPool<SelectUnitEvent> _selectionUnitPool;
        private EcsPool<UnitTeamComponent> _unitTeamPool;

        [Inject]
        private void Construct(IUnitsControlService unitsControlService, ISceneContextService sceneContextService, 
            IUIFactory uiFactory)
        {
            _unitsControlService = unitsControlService;
            _sceneContextService = sceneContextService;
            _uiFactory = uiFactory;

            _unitsControlService.UnitClicked += UnitSelected;
        }
        
        public void Dispose() => _unitsControlService.UnitClicked -= UnitSelected;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            _selectionUnitFilter = world.Filter<SelectUnitEvent>().End();
            _selectionUnitPool = world.GetPool<SelectUnitEvent>();
            _unitTeamPool = world.GetPool<UnitTeamComponent>();
        }
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _selectionUnitFilter)
            {
                ref var component = ref _selectionUnitPool.Get(entity);
                ref var unitTeamComponent = ref _unitTeamPool.Get(entity);
                var range = unitTeamComponent.UnitModel.AvailableMovementRange;
                
                var model = component.Unit.Model;
                
                _sceneContextService.GridMap.ShowPath(model.TeamType, model.CellData, range);
                _selectionUnitPool.Del(entity);
            }
        }
        
        private void UnitSelected(Unit unit)
        {
            if (unit is null) return;
            
            _sceneContextService.GridMap.HidePath();
            _uiFactory.ShowWindow(new GameHudWindowData());

            var entity = unit.EntityID;
            ref var teamComponent = ref _unitTeamPool.Get(entity);
            
            if (!teamComponent.IsActiveTeam) return;
            
            ref var selectComponent = ref _selectionUnitPool.Add(entity);
            selectComponent = new SelectUnitEvent(unit);

            var unitData = new GameHudWindowData.UnitData(unit.Model);
            _uiFactory.ShowWindow(new GameHudWindowData(unitData));
        }
    }
}