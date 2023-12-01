using System;
using System.Collections.Generic;
using System.Linq;
using Common.UnityLogic.Builders.Grid;
using Common.UnityLogic.Builders.Units;
using Common.UnityLogic.Units;
using Unity.VisualScripting;
using UnityEngine;
using Unit = Common.UnityLogic.Units.Unit;

namespace Common.Infrastructure.Services.SceneContext
{
    public sealed class GridMap
    {
        private class PathNode
        {
            public readonly Vector2Int NodeData;
            public readonly int Range;

            public PathNode(in Vector2Int nodeData, in int range)
            {
                NodeData = nodeData;
                Range = range;
            }
        }

        private class Way
        {
            public readonly List<PathNode> Path;

            public int Lenght => Path.Count;

            public Way() => Path = new List<PathNode>();
            public Way(in Way way) => Path = new List<PathNode>(way.Path);
        }

        private readonly UnitsBuilder _unitsBuilder;
        private readonly Dictionary<Vector2Int, Cell> _cellsMap;
        private readonly HashSet<Vector2Int> _availablePaths;

        public GridMap(in UnitsBuilder unitsBuilder, in IEnumerable<CellBuilder> cellBuilders)
        {
            _unitsBuilder = unitsBuilder;
            
            var cells = cellBuilders.Select(x => x.Cell);
            _cellsMap = cells.ToDictionary(x => x.Data, y => y);
            _availablePaths = new HashSet<Vector2Int>();
        }

        public bool TryGetCell(in Vector2Int data, out Cell cell)
        {
            if (_cellsMap.TryGetValue(data, out cell))
            {
                return true;
            }

            return false;
        }

        public bool CellAvailable(in Vector2Int data) => _availablePaths.Contains(data);

        public List<Cell> GetPath(in TeamTypes teamType, in Vector2Int from, in Vector2Int to)
        {
            var pathList = GetPathList(teamType, from, to);
            return pathList.Select(x => _cellsMap[x]).ToList();
        }

        public void ShowPath(in TeamTypes teamType, in Vector2Int from, in int range)
        {
            var availablePaths = CalculateAllAvailablePaths(teamType, from, range);
            _availablePaths.AddRange(availablePaths);
            
            foreach (var node in availablePaths)
            {
                if (_cellsMap.TryGetValue(node, out var cell))
                {
                    cell.ShowAvailablePath();
                }
            }
        }

        public void HidePath()
        {
            foreach (var cellData in _availablePaths)
            {
                if (_cellsMap.TryGetValue(cellData, out var cell))
                {
                    cell.HideAvailablePath();
                }
            }
            _availablePaths.Clear();
        }

        public bool IsEnemyLocated(TeamTypes teamType, Vector2Int cellData, out Unit enemyUnit)
        {
            enemyUnit = null;
            var locatedUnits = _unitsBuilder.Units.Where(x => x.Model.CellData == cellData).ToArray();
            if (!locatedUnits.Any()) return false;
            
            if (locatedUnits.Length > 1) throw new Exception("More than one unit has the same cell data");
            enemyUnit = locatedUnits[0];
            return true;
        }


        private List<Vector2Int> GetPathList(TeamTypes teamType, Vector2Int from, Vector2Int to)
        {
            Dictionary<Vector2Int, Way> availableWays = new();
            var startNode = new PathNode(from, 0);
            var way = new Way();
            CalculateAllPaths(startNode, way);

            if (IsEnemyLocated(teamType, to))
            {
                var resultPath = availableWays[to].Path;
                resultPath.RemoveAt(resultPath.Count - 1);
            }

            void CalculateAllPaths(in PathNode startPoint, in Way previousWay)
            {
                CalculatePath(startPoint.NodeData + new Vector2Int(0, 1), previousWay);
                CalculatePath(startPoint.NodeData + new Vector2Int(-1, 0), previousWay);
                CalculatePath(startPoint.NodeData + new Vector2Int(1, 0), previousWay);
                CalculatePath(startPoint.NodeData + new Vector2Int(0, -1), previousWay);

                CalculatePath(startPoint.NodeData + new Vector2Int(-1, 1), previousWay);
                CalculatePath(startPoint.NodeData + new Vector2Int(1, 1), previousWay);
                CalculatePath(startPoint.NodeData + new Vector2Int(-1, -1), previousWay);
                CalculatePath(startPoint.NodeData + new Vector2Int(1, -1), previousWay);
            }
            
            void CalculatePath(in Vector2Int newPathPoint, in Way previousWay)
            {
                if (newPathPoint != startNode.NodeData && _cellsMap.ContainsKey(newPathPoint))
                {
                    var pathNode = new PathNode(newPathPoint, previousWay.Lenght + 1);
                    if (newPathPoint != to && AnyLocated(newPathPoint)) return;
                    
                    if (!availableWays.TryGetValue(newPathPoint, out var way) || way.Lenght > pathNode.Range)
                    {
                        var newWay = new Way(previousWay);
                        newWay.Path.Add(pathNode);
                        availableWays[newPathPoint] = newWay;
                        if (newPathPoint != to) CalculateAllPaths(pathNode, newWay);
                    }
                }
            }

            return availableWays[to].Path.Select(x => x.NodeData).ToList();
        }

        private List<Vector2Int> CalculateAllAvailablePaths(TeamTypes teamType, Vector2Int from, int range)
        {
            Dictionary<Vector2Int, PathNode> availableNodes = new();

            var startNode = new PathNode(from, 0);
            CalculateAllPaths(startNode);

            void CalculateAllPaths(in PathNode fromNode)
            {
                CalculatePath(fromNode.NodeData + new Vector2Int(-1, 1), fromNode.Range);
                CalculatePath(fromNode.NodeData + new Vector2Int(0, 1), fromNode.Range);
                CalculatePath(fromNode.NodeData + new Vector2Int(1, 1), fromNode.Range);
                CalculatePath(fromNode.NodeData + new Vector2Int(-1, 0), fromNode.Range);
                CalculatePath(fromNode.NodeData + new Vector2Int(1, 0), fromNode.Range);
                CalculatePath(fromNode.NodeData + new Vector2Int(-1, -1), fromNode.Range);
                CalculatePath(fromNode.NodeData + new Vector2Int(0, -1), fromNode.Range);
                CalculatePath(fromNode.NodeData + new Vector2Int(1, -1), fromNode.Range);
            }

            return availableNodes.Select(x => x.Key).ToList();
            
            void CalculatePath(in Vector2Int to, in int previousRange)
            {
                if (IsTeammateLocated(teamType, to)) return;
                if (previousRange >= range) return;
                
                if (to != startNode.NodeData && _cellsMap.ContainsKey(to) && !IsTeammateLocated(teamType, to))
                {
                    var pathNode = new PathNode(to, previousRange + 1);
                    
                    if (!availableNodes.TryGetValue(to, out var oldNode) || oldNode.Range > pathNode.Range)
                    {
                        availableNodes[to] = pathNode;
                        CalculateAllPaths(pathNode);
                    }
                }
            }
        }

        private bool AnyLocated(Vector2Int cellData) => 
            _unitsBuilder.Units.Any(x => x.Model.CellData == cellData);

        private bool IsTeammateLocated(TeamTypes teamType, Vector2Int cellData)
        {
            var units = _unitsBuilder.Units.Where(x => x.Model.CellData == cellData).ToArray();
            if (!units.Any()) return false;
            
            return units.Any(x => x.Model.TeamType == teamType);
        }
        
        private bool IsEnemyLocated(TeamTypes teamType, Vector2Int cellData)
        {
            var units = _unitsBuilder.Units.Where(x => x.Model.CellData == cellData).ToArray();
            if (!units.Any()) return false;
            
            return units.Any(x => x.Model.TeamType != teamType);
        }
    }
}