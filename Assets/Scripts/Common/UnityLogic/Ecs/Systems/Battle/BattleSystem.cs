using System;
using Common.Infrastructure.Factories.UIFactory;
using Common.Infrastructure.Services.SceneContext;
using Common.UnityLogic.Ecs.Components.Units;
using Common.UnityLogic.Ecs.OneFrames;
using Common.UnityLogic.UI.Windows.GameHUD;
using Common.UnityLogic.UI.Windows.WinScreen;
using Common.UnityLogic.Units;
using Leopotam.EcsLite;
using Zenject;

namespace Common.UnityLogic.Ecs.Systems.Battle
{
    /// <summary>
    /// Смена и завершение хода
    /// </summary>
    public sealed class BattleSystem : IEcsInitSystem, IEcsRunSystem
    {
        public const TeamTypes DefaultTeam = TeamTypes.TeamA;

        private IUIFactory _uiFactory;
        private ISceneContextService _sceneContextService;

        private EcsFilter _enableBattleSystemFilter;
        private EcsPool<EnableBattleSystemEvent> _enableBattleSystemPool;
        
        private EcsFilter _disableBattleSystemFilter;
        private EcsPool<DisableBattleSystemEvent> _disableBattleSystemPool;
        
        private EcsFilter _completeMoveFilter;
        private EcsPool<NextTurnEvent> _completeMovePool;

        private EcsFilter _teamsFilter;
        private EcsPool<UnitTeamComponent> _teamsPool;

        private TeamTypes _activeTeam;
        private bool _isActive;

        [Inject]
        private void Construct(IUIFactory uiFactory, ISceneContextService sceneContextService)
        {
            _uiFactory = uiFactory;
            _sceneContextService = sceneContextService;
        }

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            _enableBattleSystemFilter = world.Filter<EnableBattleSystemEvent>().End();
            _enableBattleSystemPool = world.GetPool<EnableBattleSystemEvent>();

            _disableBattleSystemFilter = world.Filter<DisableBattleSystemEvent>().End();
            _disableBattleSystemPool = world.GetPool<DisableBattleSystemEvent>();
            
            _completeMoveFilter = world.Filter<NextTurnEvent>().End();
            _completeMovePool = world.GetPool<NextTurnEvent>();
            
            _teamsFilter = world.Filter<UnitTeamComponent>().End();
            _teamsPool = world.GetPool<UnitTeamComponent>();

            ResetActiveTeam();
        }

        public void Run(IEcsSystems systems)
        {
            UpdateSystemActivityState();
            if (!_isActive) return;
            
            var movesCompleted = true;
            var hasAliveEnemies = false;
            var hasMovingUnit = false;
            foreach (var entity in _teamsFilter)
            {
                ref var teamComponent = ref _teamsPool.Get(entity);

                if (teamComponent.IsActiveTeam)
                {
                    if (teamComponent.UnitModel.IsMoving) hasMovingUnit = true;
                    if (teamComponent.UnitModel.HasAvailableRange) movesCompleted = false;
                }
                else
                {
                    if (teamComponent.UnitModel.IsAlive) hasAliveEnemies = true;
                }
            }

            if (hasAliveEnemies)
            {
                // check event complete move
                foreach (var entity in _completeMoveFilter)
                {
                    _completeMovePool.Del(entity);
                    movesCompleted = true;
                }

                if (!hasMovingUnit && movesCompleted) ChangeTeam();
            }
            else
            {
                _uiFactory.Hide(new GameHudWindowData());
                _uiFactory.ShowWindow(new WinWindowData(_activeTeam));
                ResetActiveTeam();
            }
        }

        private void UpdateSystemActivityState()
        {
            foreach (var entity in _disableBattleSystemFilter)
            {
                _disableBattleSystemPool.Del(entity);
                _isActive = false;
            }

            foreach (var entity in _enableBattleSystemFilter)
            {
                _enableBattleSystemPool.Del(entity);
                _isActive = true;
            }
        }

        private void ChangeTeam()
        {
            var teamIndex = (int)_activeTeam;
            teamIndex++;

            if (teamIndex >= Enum.GetValues(typeof(TeamTypes)).Length) teamIndex = 0;

            _activeTeam = (TeamTypes)teamIndex;

            UpdateHudAndHidePath();
            UpdateUnitTeamComponents();
        }

        private void UpdateHudAndHidePath()
        {
            _uiFactory.ShowWindow(new GameHudWindowData());
            _sceneContextService.GridMap.HidePath();
        }

        private void UpdateUnitTeamComponents()
        {
            foreach (var entity in _teamsFilter)
            {
                ref var teamComponent = ref _teamsPool.Get(entity);
                var isActiveTeam = _activeTeam == teamComponent.UnitModel.TeamType;
                teamComponent.IsActiveTeam = isActiveTeam;
                teamComponent.UnitModel.AvailableMovementRange = isActiveTeam ? teamComponent.UnitModel.StaticData.Range : 0;
            }
        }

        private void ResetActiveTeam() => _activeTeam = DefaultTeam;
    }
}