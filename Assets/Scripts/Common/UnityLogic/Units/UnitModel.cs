using Common.StaticData;
using UnityEngine;

namespace Common.UnityLogic.Units
{
    public sealed class UnitModel
    {
        public readonly UnitStaticData StaticData;
        public readonly TeamTypes TeamType;
        
        public Vector2Int CellData { get; set; }

        public UnitModel(in UnitStaticData staticData, in TeamTypes teamType, in Vector2Int cellData)
        {
            StaticData = staticData;
            TeamType = teamType;
            CellData = cellData;
        }
    }
}