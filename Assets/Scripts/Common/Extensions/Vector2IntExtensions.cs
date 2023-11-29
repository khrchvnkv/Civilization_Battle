using UnityEngine;

namespace Common.Extensions
{
    public static class Vector2IntExtensions
    {
        public static Vector2Int LimitToPositiveValues(this Vector2Int vector2Int)
        {
            if (vector2Int.x < 0) vector2Int.x = 0;
            if (vector2Int.y < 0) vector2Int.y = 0;

            return vector2Int;
        }
        
        public static Vector2Int LimitMinValues(this Vector2Int vector2Int, int minXValue, int minYValue)
        {
            if (vector2Int.x < minXValue) vector2Int.x = minXValue;
            if (vector2Int.y < minYValue) vector2Int.y = minYValue;

            return vector2Int;
        }
    }
}