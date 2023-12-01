using System.Collections.Generic;
using Common.StaticData;
using Common.UnityLogic.Builders.Grid;
using Common.UnityLogic.Ecs.Providers.Unit;
using UnityEngine;

namespace Common.UnityLogic.Units
{
    [RequireComponent(typeof(UnitProvider))]
    [RequireComponent(typeof(UnitView))]
    [RequireComponent(typeof(UnitMovement))]
    public sealed class Unit : MonoBehaviour
    {
        [SerializeField] private UnitProvider _unitProvider;
        [SerializeField] private UnitView _unitView;
        [SerializeField] private UnitMovement _unitMovement;

        public int EntityID => _unitProvider.EntityID;
        public bool IsMoving => _unitMovement.IsMoving;
        public UnitModel Model { get; private set; }

        private void OnValidate()
        {
            _unitProvider ??= gameObject.GetComponent<UnitProvider>();
            _unitView ??= gameObject.GetComponent<UnitView>();
            _unitMovement ??= gameObject.GetComponent<UnitMovement>();
            
            // Disable by default; enable on init
            _unitProvider.enabled = false;
        }

        public void Init(in UnitStaticData staticData, in TeamTypes teamType, in Vector2Int cellData)
        {
            Model = new UnitModel(staticData, teamType, cellData);
            _unitView.UpdateView(teamType);

            _unitProvider.enabled = true;
        }

        public void MoveUnit(in List<Cell> cells) => _unitMovement.MoveUnit(cells, Model);

        private void OnDisable() => _unitProvider.enabled = false;
    }
}