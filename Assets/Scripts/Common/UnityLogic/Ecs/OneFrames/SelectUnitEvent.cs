using Common.UnityLogic.Units;

namespace Common.UnityLogic.Ecs.OneFrames
{
    public struct SelectUnitEvent
    {
        public readonly Unit Unit;

        public SelectUnitEvent(Unit unit)
        {
            Unit = unit;
        }
    }
}