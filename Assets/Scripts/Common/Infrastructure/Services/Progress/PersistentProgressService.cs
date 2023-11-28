using Common.Infrastructure.Services.SavedData;

namespace Common.Infrastructure.Services.Progress
{
    public sealed class PersistentProgressService : IPersistentProgressService
    {
        public SaveData SaveData { get; set; }
    }
}