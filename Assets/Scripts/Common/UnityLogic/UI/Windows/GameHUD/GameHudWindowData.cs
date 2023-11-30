using Common.Infrastructure.WindowsManagement;

namespace Common.UnityLogic.UI.Windows.GameHUD
{
    public struct GameHudWindowData : IWindowData
    {
        public string WindowName => "GameHUD";
        public bool DestroyOnClosing => true;
    }
}