using System.Collections.Generic;
using Common.UnityLogic.Builders.Grid;
using DG.Tweening;
using UnityEngine;

namespace Common.UnityLogic.Units
{
    public sealed class UnitMovement : MonoBehaviour
    {
        private const float MovementBtwCellsDuration = 0.5f;

        [SerializeField] private Transform _transform;
        
        public bool IsMoving { get; private set; }

        private void OnValidate() => _transform ??= transform;

        public void MoveUnit(List<Cell> cells, UnitModel unitModel)
        {
            IsMoving = true;
            MoveTween();

            void MoveTween() => 
                _transform
                    .DOMove(cells[0].UnitSpawnPoint.position, MovementBtwCellsDuration)
                    .SetEase(Ease.Linear)
                    .OnComplete(MoveToNextPoint);
            
            void MoveToNextPoint()
            {
                var cell = cells[0];
                cells.Remove(cell);
                if (cells.Count == 0)
                {
                    // Complete moving
                    _transform.position = cell.UnitSpawnPoint.position;
                    _transform.SetParent(cell.UnitSpawnPoint);
                    unitModel.CellData = cell.Data;
                    IsMoving = false;
                }
                else
                {
                    // Move to next cell
                    MoveTween();
                }
            }
        }
    }
}