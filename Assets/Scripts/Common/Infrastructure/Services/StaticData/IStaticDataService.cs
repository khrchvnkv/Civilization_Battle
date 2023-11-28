using Common.StaticData;

namespace Common.Infrastructure.Services.StaticData
{
    public interface IStaticDataService
    {
        GameStaticData GameStaticData { get; }

        void Load();
    }
}