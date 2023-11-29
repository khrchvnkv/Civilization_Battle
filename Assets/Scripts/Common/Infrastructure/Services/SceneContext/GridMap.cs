using System;
using System.Collections.Generic;
using System.Linq;
using Common.UnityLogic.Builders.Grid;
using UnityEngine;

namespace Common.Infrastructure.Services.SceneContext
{
    public sealed class GridMap
    {
        private Dictionary<Vector2Int, Cell> _cellsMap;

        public GridMap(in IEnumerable<Cell> cells) => _cellsMap = cells.ToDictionary(x => x.Data, y => y);

        public Cell GetCell(in Vector2Int data)
        {
            if (_cellsMap.TryGetValue(data, out var cell))
            {
                return cell;
            }

            throw new Exception($"No cell with data {data}");
        }
    }
}