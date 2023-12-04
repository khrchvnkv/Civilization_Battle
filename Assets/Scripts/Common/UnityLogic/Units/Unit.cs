using System;
using System.Collections.Generic;
using Common.Infrastructure.Services.ECS;
using Common.Infrastructure.Services.Input;
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
        private IInputService _inputService;
        
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
        private void Construct(IEcsStartup ecsStartup, IInputService inputService)
        {
            _ecsStartup = ecsStartup;
            _inputService = inputService;
        }

        public void Init(in UnitStaticData staticData, in TeamTypes teamType, in Vector2Int cellData)
        {
            Model = new UnitModel(staticData, teamType, cellData, _unitHealth, _unitView, _unitMovement);
            _unitHealth.Setup(Model.StaticData.HP);

            EnableEcsProvider();
            EnableCollider();
        }

        public void MoveUnit(in List<Cell> cells, Unit attackedUnit = null)
        {
            _unitView.PlayMoveAnimation();
            _unitMovement.MoveUnit(cells, Model, () =>
            {
                _unitView.PlayIdleAnimation();
                if (attackedUnit is not null) Attack(attackedUnit);
            });
        }

        private bool IsActiveTeam()
        {
            ref var component = ref _ecsStartup.World.GetPool<UnitTeamComponent>().Get(EntityID);
            return component.IsActiveTeam;
        }
        
        private void TakeDamage(in float damage)
        {
            _unitHealth.TakeDamage(damage);
            if (_unitHealth.IsAlive)
            {
                _unitView.PlayHitAnimation();
            }
            else
            {
                _unitView.PlayDieAnimation();
            }
        }

        private void Attack(in Unit attackedUnit)
        {
            if (attackedUnit is null) throw new Exception("The attacked unit can not be null");

            _unitMovement.LookToEachOther(attackedUnit._unitMovement);
            
            _unitView.PlayAttackAnimation();
            var damageMultiplier = GridMap.IsDiagonalCells(Model.CellData, attackedUnit.Model.CellData)
                ? Model.StaticData.DiagonalAttackMultiplier
                : 1.0f;
            attackedUnit.TakeDamage(damageMultiplier * Model.StaticData.Damage);
            Model.AvailableMovementRange = 0;
        }

        private void OnEnable()
        {
            _inputService.UnitClicked += UpdateSelectedView;
            _unitHealth.Died += UnitDied;
        }

        private void OnDisable()
        {
            _inputService.UnitClicked -= UpdateSelectedView;
            _unitHealth.Died -= UnitDied;
            DisableEcsProvider();
        }

        private void UnitDied()
        {
            DisableEcsProvider();
            DisableCollider();
        }

        private void UpdateSelectedView(Unit unit) => _unitView.SetSelectedViewActivity(unit == this);

        private void EnableEcsProvider() => _unitProvider.enabled = true;
        
        private void DisableEcsProvider() => _unitProvider.enabled = false;

        private void EnableCollider() => _collider.enabled = true;
        
        private void DisableCollider() => _collider.enabled = false;
    }
}