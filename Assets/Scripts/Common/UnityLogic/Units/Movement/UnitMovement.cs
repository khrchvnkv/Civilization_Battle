using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Common.UnityLogic.Builders.Grid;
using DG.Tweening;
using UnityEngine;

namespace Common.UnityLogic.Units.Movement
{
    public sealed class UnitMovement : MonoBehaviour
    {
        private const float MovementBtwCellsDuration = 0.5f;

        [SerializeField] private Transform _transform;

        private Tween _tween;
        
        public bool IsMoving { get; private set; }

        private void OnValidate() => _transform ??= transform;

        public void LookToEachOther(in UnitMovement targetMovement)
        {
            var direction = targetMovement._transform.position - _transform.position;
            _transform.rotation = Quaternion.LookRotation(direction);
            targetMovement._transform.rotation = Quaternion.LookRotation(-direction);
        }
        
        public void MoveUnit(List<Cell> cells, UnitModel unitModel, Action completePathAction = null)
        {
            if (!cells.Any())
            {
                completePathAction?.Invoke();
                return;
            }

            var lastCell = cells[^1];
            unitModel.CellData = lastCell.Data;
            IsMoving = true;
            MoveTween();

            void MoveTween()
            {
                var direction = cells[0].UnitSpawnPoint.position - _transform.position;
                _transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
                _tween = _transform
                    .DOMove(cells[0].UnitSpawnPoint.position, MovementBtwCellsDuration)
                    .SetEase(Ease.Linear)
                    .OnComplete(MoveToNextPoint);
            }

            void MoveToNextPoint()
            {
                var cell = cells[0];
                cells.Remove(cell);
                if (cells.Count == 0)
                {
                    // Complete moving
                    _transform.position = cell.UnitSpawnPoint.position;
                    _transform.SetParent(cell.UnitSpawnPoint);
                    completePathAction?.Invoke();
                    IsMoving = false;
                }
                else
                {
                    // Move to next cell
                    MoveTween();
                }
            }
        }

        private void OnDestroy() => _tween?.Kill();
    }
}