namespace Common.Infrastructure.Services.SavedData
{
    public class SaveData
    {
        public ProgressData Progress;
        public SettingsData Settings;

        public SaveData()
        {
            Progress = new ProgressData();
            Settings = new SettingsData();
        }
    }
}