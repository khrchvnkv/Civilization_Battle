using Common.Infrastructure.WindowsManagement;

namespace Common.UnityLogic.UI.Windows
{
    public interface IWindow
    {
        void Show(IWindowData windowData);
        void Hide();
    }
}