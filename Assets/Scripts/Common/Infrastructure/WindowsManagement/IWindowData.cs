namespace Common.Infrastructure.WindowsManagement
{
    public interface IWindowData
    {
        string WindowName { get; }
        bool DestroyOnClosing { get; }
    }
}