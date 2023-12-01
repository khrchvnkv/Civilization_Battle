using System.Collections.Generic;
using Common.Infrastructure.Services.ECS;
using Common.StaticData;
using Common.UnityLogic.Builders.Grid;
using Common.UnityLogic.Ecs.Components.Units;
using Common.UnityLogic.Ecs.Providers.Unit;
using Common.UnityLogic.Units.Health;
using Common.UnityLogic.Units.Movement;
using Common.UnityLogic.Units.View;
using UnityEngine;
using Zenject;

namespace Common.UnityLogic.Units
{
    [RequireComponent(typeof(UnitProvider))]
    [RequireComponent(typeof(UnitView))]
    [RequireComponent(typeof(UnitMovement))]
    [RequireComponent(typeof(UnitHealth))]
    public sealed class Unit : MonoBehaviour
    {
        [SerializeField] private UnitProvider _unitProvider;
        [SerializeField] private UnitView _unitView;
        [SerializeField] private UnitMovement _unitMovement;
        [SerializeField] private UnitHealth _unitHealth;

        private IEcsStartup _ecsStartup;
        
        public int EntityID => _unitProvider.EntityID;
        public bool IsAvailable => _unitHealth.IsAlive && !_unitMovement.IsMoving && IsActiveTeam();
        public UnitModel Model { get; private set; }

        private void OnValidate()
        {
            _unitProvider ??= gameObject.GetComponent<UnitProvider>();
            _unitView ??= gameObject.GetComponent<UnitView>();
            _unitMovement ??= gameObject.GetComponent<UnitMovement>();
            _unitHealth ??= gameObject.GetComponent<UnitHealth>();
            
            // Disable by default; enable on init
            DisableEcsProvider();
        }

        [Inject]
        private void Construct(IEcsStartup ecsStartup) => _ecsStartup = ecsStartup;

        public void Init(in UnitStaticData staticData, in TeamTypes teamType, in Vector2Int cellData)
        {
            Model = new UnitModel(staticData, teamType, cellData, _unitHealth, _unitMovement);
            _unitHealth.Init(Model.StaticData.HP);
            _unitView.UpdateView(teamType);

            EnableEcsProvider();
        }

        public void MoveUnit(in List<Cell> cells, Unit attackedUnit = null)
        {
            _unitMovement.MoveUnit(cells, Model, () =>
            {
                if (attackedUnit is not null) Attack(attackedUnit);
            });
        }

        private bool IsActiveTeam()
        {
            ref var component = ref _ecsStartup.World.GetPool<UnitTeamComponent>().Get(EntityID);
            return component.IsActiveTeam;
        }
        
        private void TakeDamage(in float damage) => _unitHealth.TakeDamage(damage);

        private void Attack(in Unit attackedUnit) => attackedUnit?.TakeDamage(Model.StaticData.Damage);

        private void OnEnable() => _unitHealth.Died += UnitDied;

        private void OnDisable()
        {
            _unitHealth.Died -= UnitDied;
            DisableEcsProvider();
        }

        private void UnitDied()
        {
            DisableEcsProvider();
        }

        private void EnableEcsProvider() => _unitProvider.enabled = true;
        
        private void DisableEcsProvider() => _unitProvider.enabled = false;
    }
}