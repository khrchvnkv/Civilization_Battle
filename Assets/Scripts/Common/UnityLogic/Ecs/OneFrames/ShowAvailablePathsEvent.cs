using UnityEngine;

namespace Common.UnityLogic.Ecs.OneFrames
{
    public struct ShowAvailablePathsEvent
    {
        public readonly Vector2Int StartPosition;
        public readonly int Range;

        public ShowAvailablePathsEvent(Vector2Int startPosition, int range)
        {
            StartPosition = startPosition;
            Range = range;
        }
    }
}