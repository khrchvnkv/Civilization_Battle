using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace Common.UnityLogic.GridLogic
{
    public sealed class GridBuilder : MonoBehaviour
    {
        private const int MinGridValue = 3;
        private const float Offset = 0.5f;
        
        [SerializeField] private Vector2Int _gridSize;
        [SerializeField] private Cell _cellPrefab;
        [SerializeField] private List<Cell> _cells;

        private float CellSize => _cellPrefab.CellSize;
        
        private void OnValidate()
        {
            if (_gridSize.x < MinGridValue) _gridSize.x = MinGridValue;
            if (_gridSize.y < MinGridValue) _gridSize.y = MinGridValue;
        }
        
        [Button(nameof(Build))]
        private void Build()
        {
            Clear();
            var startPoint = transform.position;
            startPoint.x -= (CellSize + Offset) / 2 * (_gridSize.x - 1);
            startPoint.z -= (CellSize + Offset) / 2 * (_gridSize.y - 1);
                
            for (int i = 0; i < _gridSize.x; i++)
            {
                for (int j = 0; j < _gridSize.y; j++)
                {
                    var point = startPoint + new Vector3(i * (CellSize + Offset), 0, j * (CellSize + Offset));
                    var instance = Instantiate(_cellPrefab, point, Quaternion.identity, transform);
                    _cells.Add(instance);
                }
            }
        }
        private void Clear()
        {
            foreach (var cell in _cells)
            {
                DestroyImmediate(cell.gameObject);
            }
            _cells.Clear();
        }
    }
}