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
                if (hit.collider.gameObject.TryGetComponent(out Cell cell))
                {
                    if (cell != _hoveredCell) HoverCell(cell);
                }
                else
                {
                    UnhoverCell();
                }

                if (UnityEngine.Input.GetMouseButtonDown(0))
                {
                    if (hit.collider.gameObject.TryGetComponent(out Unit unit) && unit != _selectedUnit)
                    {
                        if (unit.IsAvailable)
                        {
                            _selectedUnit = unit;
                            InvokeUnitSelected();
                            return;
                        }
                        
                        // Try attack
                        var unitCellData = unit.Model.CellData;
                        if (_selectedUnit is not null && 
                            _sceneContextService.GridMap.TryGetCell(unitCellData, out var unitCell))
                        {
                            HoverCell(unitCell);
                        }
                    }

                    if (_selectedUnit is not null && _hoveredCell is not null)
                    {
                        UnitMoveClicked?.Invoke(_selectedUnit, _hoveredCell);
                        _selectedUnit = null;
                        InvokeUnitSelected();
                    }
                }
            }
            else
            {
                UnhoverCell();
            }
        }

        private void InvokeUnitSelected() => UnitClicked?.Invoke(_selectedUnit);

        private void HoverCell(in Cell cell)
        {
            UnhoverCell();
            _hoveredCell = cell;
            cell.View.SetHovered();
        }
        
        private void UnhoverCell()
        {
            _hoveredCell?.View.SetUnhovered();
            _hoveredCell = null;
        }
    }
}