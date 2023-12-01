using Common.UnityLogic.Builders.Grid;
using Common.UnityLogic.Units;

namespace Common.UnityLogic.Ecs.Components.Units
{
    public struct UnitMovementComponent
    {
        public readonly Unit Unit;
        public readonly Cell MoveTo;

        public UnitMovementComponent(Unit unit, Cell moveTo)
        {
            Unit = unit;
            MoveTo = moveTo;
        }
    }
}