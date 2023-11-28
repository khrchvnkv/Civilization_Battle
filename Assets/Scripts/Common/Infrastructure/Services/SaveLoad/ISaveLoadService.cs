using Common.Infrastructure.Services.SavedData;

namespace Common.Infrastructure.Services.SaveLoad
{
    public interface ISaveLoadService
    {
        void SaveData();
        SaveData LoadData();
    }
}