using Common.Infrastructure.Services.SavedData;

namespace Common.Infrastructure.Services.Progress
{
    public interface IPersistentProgressService
    {
        SaveData SaveData { get; set; }
    }
}