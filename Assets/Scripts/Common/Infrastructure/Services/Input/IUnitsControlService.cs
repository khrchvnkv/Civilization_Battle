using System;
using Common.UnityLogic.Builders.Grid;
using Common.UnityLogic.Units;

namespace Common.Infrastructure.Services.Input
{
    public interface IUnitsControlService
    {
        event Action<Unit> UnitClicked;
        event Action<Unit, Cell> UnitMoveClicked;

        void SelectNextAvailableUnit();

        void Enable();
        void Disable();
    }
}