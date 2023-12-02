using Common.Infrastructure.WindowsManagement;
using Common.UnityLogic.Units;

namespace Common.UnityLogic.UI.Windows.WinScreen
{
    public struct WinWindowData : IWindowData
    {
        public readonly TeamTypes WinningTeam;

        public string WindowName => "WinWindow";
        public bool DestroyOnClosing => true;
        
        public WinWindowData(TeamTypes winningTeam) => WinningTeam = winningTeam;
    }
}