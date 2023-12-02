using Common.StaticData;
using Common.UnityLogic.Units.Health;
using Common.UnityLogic.Units.View;
using UnityEngine;

namespace Common.UnityLogic.Units
{
    public sealed class UnitModel
    {
        public readonly UnitStaticData StaticData;
        public readonly TeamTypes TeamType;

        private readonly UnitHealth _unitHealth;
        private readonly UnitView _unitView;

        private int _availableMovementRange;
        
        public Vector2Int CellData { get; set; }

        public int AvailableMovementRange
        {
            get => _availableMovementRange;
            set
            {
                _availableMovementRange = value;
                var hasAvailableRange = HasAvailableRange;
                _unitView.SetAvailableRangeActivity(hasAvailableRange);
                if (!hasAvailableRange) _unitView.SetSelectedViewActivity(false);
            }
        }

        public float HP => _unitHealth.HP;
        public bool IsAlive => HP > 0;
        public bool HasAvailableRange => AvailableMovementRange > 0;

        public UnitModel(in UnitStaticData staticData, in TeamTypes teamType, 
            in Vector2Int cellData, in UnitHealth unitHealth, in UnitView unitView)
        {
            StaticData = staticData;
            TeamType = teamType;
            CellData = cellData;
            _unitHealth = unitHealth;
            _unitView = unitView;
        }
    }
}