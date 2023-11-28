using Common.Extensions;
using Common.Infrastructure.Services.Progress;
using Common.Infrastructure.Services.SavedData;
using UnityEngine;

namespace Common.Infrastructure.Services.SaveLoad
{
    public sealed class SaveLoadService : ISaveLoadService
    {
        private const string SAVE_DATA_KEY = "SaveDataKey";

        private readonly IPersistentProgressService _progressService;
        
        public SaveLoadService(IPersistentProgressService progressService)
        {
            _progressService = progressService;
        }
        public void SaveData() => 
            PlayerPrefs.SetString(SAVE_DATA_KEY, _progressService.SaveData.Serialize());
        public SaveData LoadData() => 
            PlayerPrefs.GetString(SAVE_DATA_KEY, null)?.Deserialize<SaveData>();
    }
}