using System.Collections.Generic;
using Common.Extensions;
using NaughtyAttributes;
using UnityEngine;

namespace Common.UnityLogic.Builders.Grid
{
    public sealed class GridBuilder : MonoBehaviour
    {
        private const int MinGridValue = 3;
        
        [SerializeField] private Vector2Int _gridSize;
        [SerializeField] private float _offset;
        [SerializeField] private float _cellSize;
        [SerializeField] private Cell _cellPrefab;
        [SerializeField] private List<Cell> _cells;

        public IEnumerable<Cell> Cells => _cells;
        
        private void OnValidate()
        {
            _gridSize.LimitMinValues(MinGridValue, MinGridValue);
            if (_offset < 0) _offset = 0;
            if (_cellSize < 0) _cellSize = 0;
        }
        
        [Button]
        private void Build()
        {
            Clear();
            var startPoint = transform.position;
            startPoint.x -= (_cellSize + _offset) / 2 * (_gridSize.x - 1);
            startPoint.z -= (_cellSize + _offset) / 2 * (_gridSize.y - 1);
                
            for (int i = 0; i < _gridSize.y; i++)
            {
                for (int j = 0; j < _gridSize.x; j++)
                {
                    var point = startPoint + new Vector3(j * (_cellSize + _offset), 0, i * (_cellSize + _offset));
                    var instance = Instantiate(_cellPrefab, point, Quaternion.identity, transform);
                    instance.name = $"Cell_{j}_{i}";
                    instance.Data = new Vector2Int(j, i);
                    _cells.Add(instance);
                }
            }
        }
        
        private void Clear()
        {
            foreach (var cell in _cells)
            {
                if (cell is not null) DestroyImmediate(cell.gameObject);
            }
            _cells.Clear();
        }
    }
}