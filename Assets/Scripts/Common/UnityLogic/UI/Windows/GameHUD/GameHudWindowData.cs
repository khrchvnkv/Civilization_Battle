using Common.Infrastructure.WindowsManagement;

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
            
        }
    }
}