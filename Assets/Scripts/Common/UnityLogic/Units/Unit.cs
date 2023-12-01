using System;
using System.Collections.Generic;
using Common.Infrastructure.Services.ECS;
using Common.Infrastructure.Services.SceneContext;
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
        [SerializeField] private Collider _collider;

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
            EnableCollider();
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

        private void Attack(in Unit attackedUnit)
        {
            if (attackedUnit is null) throw new Exception("The attacked unit can not be null");

            var damageMultiplier = GridMap.IsDiagonalCells(Model.CellData, attackedUnit.Model.CellData)
                ? Model.StaticData.DiagonalAttackMultiplier
                : 1.0f;
            attackedUnit.TakeDamage(damageMultiplier * Model.StaticData.Damage);
            Model.AvailableMovementRange = 0;
        }

        private void OnEnable() => _unitHealth.Died += UnitDied;

        private void OnDisable()
        {
            _unitHealth.Died -= UnitDied;
            DisableEcsProvider();
        }

        private void UnitDied()
        {
            DisableEcsProvider();
            DisableCollider();
        }

        private void EnableEcsProvider() => _unitProvider.enabled = true;
        
        private void DisableEcsProvider() => _unitProvider.enabled = false;

        private void EnableCollider() => _collider.enabled = true;
        
        private void DisableCollider() => _collider.enabled = false;
    }
}