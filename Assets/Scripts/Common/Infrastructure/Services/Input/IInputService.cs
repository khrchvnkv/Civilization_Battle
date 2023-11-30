using System;
using Common.UnityLogic.Builders.Grid;
using Common.UnityLogic.Units;

namespace Common.Infrastructure.Services.Input
{
    public interface IInputService
    {
        event Action<Unit> UnitClicked;
        event Action<Unit, Cell> UnitMoveClicked;

        void Enable();
        void Disable();
    }
}