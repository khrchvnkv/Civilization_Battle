using Common.StaticData;
using Common.UnityLogic.Units.Health;
using Common.UnityLogic.Units.Movement;
using UnityEngine;

namespace Common.UnityLogic.Units
{
    public sealed class UnitModel
    {
        public readonly UnitStaticData StaticData;
        public readonly TeamTypes TeamType;

        private readonly UnitHealth _unitHealth;
        private readonly UnitMovement _unitMovement;
        
        public Vector2Int CellData { get; set; }
        public float HP => _unitHealth.HP;
        public bool IsMoving => _unitMovement.IsMoving;

        public UnitModel(in UnitStaticData staticData, in TeamTypes teamType, 
            in Vector2Int cellData, in UnitHealth unitHealth, in UnitMovement unitMovement)
        {
            StaticData = staticData;
            TeamType = teamType;
            CellData = cellData;
            _unitHealth = unitHealth;
            _unitMovement = unitMovement;
        }
    }
}