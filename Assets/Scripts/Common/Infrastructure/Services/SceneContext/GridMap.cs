using System.Collections.Generic;
using System.Linq;
using Common.UnityLogic.Builders.Grid;
using Unity.VisualScripting;
using UnityEngine;

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
        
        private readonly Dictionary<Vector2Int, Cell> _cellsMap;
        private readonly HashSet<Vector2Int> _availablePaths;

        public GridMap(in IEnumerable<Cell> cells)
        {
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

        public List<Cell> GetPath(in Vector2Int from, in Vector2Int to)
        {
            var pathList = GetPathList(from, to);
            return pathList.Select(x => _cellsMap[x]).ToList();
        }

        public void ShowPath(in Vector2Int from, in int range)
        {
            var availablePaths = CalculateAllAvailablePaths(from, range);
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

        private List<Vector2Int> GetPathList(Vector2Int from, Vector2Int to)
        {
            Dictionary<Vector2Int, Way> availableWays = new();
            var startNode = new PathNode(from, 0);
            var way = new Way();
            CalculateAllPaths(startNode, way);

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

        private List<Vector2Int> CalculateAllAvailablePaths(Vector2Int from, int range)
        {
            Dictionary<Vector2Int, PathNode> availableNodes = new();

            var startNode = new PathNode(from, 0);
            CalculateAllPaths(startNode);

            void CalculateAllPaths(in PathNode from)
            {
                CalculatePath(from.NodeData + new Vector2Int(-1, 1), from.Range);
                CalculatePath(from.NodeData + new Vector2Int(0, 1), from.Range);
                CalculatePath(from.NodeData + new Vector2Int(1, 1), from.Range);
                CalculatePath(from.NodeData + new Vector2Int(-1, 0), from.Range);
                CalculatePath(from.NodeData + new Vector2Int(1, 0), from.Range);
                CalculatePath(from.NodeData + new Vector2Int(-1, -1), from.Range);
                CalculatePath(from.NodeData + new Vector2Int(0, -1), from.Range);
                CalculatePath(from.NodeData + new Vector2Int(1, -1), from.Range);
            }

            return availableNodes.Select(x => x.Key).ToList();
            
            void CalculatePath(in Vector2Int to, in int previousRange)
            {
                if (previousRange >= range) return;
                
                if (to != startNode.NodeData && _cellsMap.ContainsKey(to))
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
    }
}