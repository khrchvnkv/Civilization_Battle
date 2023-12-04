using Common.StaticData;
using Common.UnityLogic.Units.Health;
using Common.UnityLogic.Units.Movement;
using Common.UnityLogic.Units.View;
using UnityEngine;

namespace Common.UnityLogic.Units
{
    public sealed class UnitModel
    {
        public readonly UnitStaticData StaticData;
        public readonly TeamTypes TeamType;

        private readonly UnitHealth _unitHealth;
        private readonly UnitView _view;
        private readonly UnitMovement _movement;

        private int _availableMovementRange;
        
        public Vector2Int CellData { get; set; }

        public int AvailableMovementRange
        {
            get => _availableMovementRange;
            set
            {
                _availableMovementRange = value;
                var hasAvailableRange = HasAvailableRange;
                _view.SetAvailableRangeActivity(hasAvailableRange);
                if (!hasAvailableRange) _view.SetSelectedViewActivity(false);
            }
        }

        public float HP => _unitHealth.HP;
        public bool IsAlive => HP > 0;
        public bool HasAvailableRange => AvailableMovementRange > 0;
        public bool IsMoving => _movement.IsMoving;
        
        public UnitModel(in UnitStaticData staticData, in TeamTypes teamType, 
            in Vector2Int cellData, in UnitHealth unitHealth, in UnitView view, in UnitMovement movement)
        {
            StaticData = staticData;
            TeamType = teamType;
            CellData = cellData;
            _unitHealth = unitHealth;
            _view = view;
            _movement = movement;
        }
    }
}