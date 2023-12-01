using System;
using Common.Infrastructure.Services.MonoUpdate;
using Common.Infrastructure.Services.SceneContext;
using Common.UnityLogic.Builders.Grid;
using Common.UnityLogic.Units;
using UnityEngine;

namespace Common.Infrastructure.Services.Input
{
    public sealed class InputService : IInputService, IDisposable
    {
        public event Action<Unit> UnitClicked;
        public event Action<Unit, Cell> UnitMoveClicked;

        private readonly IMonoUpdateSystem _updateSystem;
        private readonly ISceneContextService _sceneContextService;

        private Unit _selectedUnit;
        private Cell _hoveredCell;
        
        private bool _isActive;

        public InputService(IMonoUpdateSystem updateSystem, ISceneContextService sceneContextService)
        {
            _updateSystem = updateSystem;
            _sceneContextService = sceneContextService;
            
            _updateSystem.OnUpdate += UpdateInput;
        }

        public void Dispose() => _updateSystem.OnUpdate -= UpdateInput;

        public void Enable() => _isActive = true;
        
        public void Disable()
        {
            _selectedUnit = null;
            UnhoverCell();
            _isActive = false;
        }

        private void UpdateInput()
        {
            if (!_isActive) return;

            RaycastHit hit;
            Ray ray = _sceneContextService.MainCamera.ScreenPointToRay(UnityEngine.Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.TryGetComponent(out Cell cell) && _hoveredCell != cell)
                {
                    HoverCell(cell);
                }

                if (UnityEngine.Input.GetMouseButtonDown(0))
                {
                    if (hit.collider.gameObject.TryGetComponent(out Unit unit) && unit != _selectedUnit && !unit.IsMoving)
                    {
                        _selectedUnit = unit;
                        UnitClicked?.Invoke(_selectedUnit);
                        return;
                    }

                    if (_selectedUnit is not null && _hoveredCell is not null)
                    {
                        UnitMoveClicked?.Invoke(_selectedUnit, _hoveredCell);
                        _selectedUnit = null;
                    }
                }
            }
        }

        private void HoverCell(in Cell cell)
        {
            UnhoverCell();
            _hoveredCell = cell;
            cell.Hover();
        }
        
        private void UnhoverCell()
        {
            _hoveredCell?.Unhover();
            _hoveredCell = null;
        }
    }
}