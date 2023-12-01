using Common.Infrastructure.WindowsManagement;
using Common.UnityLogic.Units;

namespace Common.UnityLogic.UI.Windows.GameHUD
{
    public struct GameHudWindowData : IWindowData
    {
        public string WindowName => "GameHUD";
        public bool DestroyOnClosing => true;
        public UnitData? Unit { get; }
        
        public GameHudWindowData(UnitData? unit) => Unit = unit;

        public struct UnitData
        {
            public readonly UnitModel UnitModel;
            public readonly int AvailableMovementRange;

            public UnitData(UnitModel unitModel, int availableMovementRange)
            {
                UnitModel = unitModel;
                AvailableMovementRange = availableMovementRange;
            }
        }
    }
}