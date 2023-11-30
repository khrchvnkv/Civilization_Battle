using System.Collections.Generic;
using Common.StaticData;
using Common.UnityLogic.Builders.Grid;
using Common.UnityLogic.Ecs.Providers.Unit;
using DG.Tweening;
using UnityEngine;

namespace Common.UnityLogic.Units
{
    [RequireComponent(typeof(UnitProvider))]
    public sealed class Unit : MonoBehaviour
    {
        [SerializeField] private UnitProvider _unitProvider;
        [SerializeField] private SpriteRenderer _circleSpriteRenderer;

        public int EntityID => _unitProvider.EntityID;
        public UnitStaticData StaticData { get; private set; }
        public TeamTypes TeamType { get; private set; }
        public Vector2Int CellData { get; private set; }
        public bool IsMoving { get; private set; }

        private void OnValidate()
        {
            _unitProvider ??= gameObject.GetComponent<UnitProvider>();
            _unitProvider.enabled = false;
        }

        public void Init(in UnitStaticData staticData, in TeamTypes teamType, in Vector2Int vector2Int)
        {
            StaticData = staticData;
            TeamType = teamType;
            CellData = vector2Int;
            
            _circleSpriteRenderer.color = Constants.TeamColors[teamType];

            _unitProvider.enabled = true;
        }

        public void MoveUnit(List<Cell> cells)
        {
            IsMoving = true;
            MoveTween();

            void MoveTween() => 
                transform
                    .DOMove(cells[0].UnitSpawnPoint.position, 0.5f)
                    .SetEase(Ease.Linear)
                    .OnComplete(MoveToNextPoint);
            
            void MoveToNextPoint()
            {
                var cell = cells[0];
                cells.Remove(cell);
                if (cells.Count == 0)
                {
                    // Complete moving
                    transform.position = cell.UnitSpawnPoint.position;
                    transform.SetParent(cell.UnitSpawnPoint);
                    CellData = cell.Data;
                    IsMoving = false;
                }
                else
                {
                    MoveTween();
                }
            }
        }
    }
}