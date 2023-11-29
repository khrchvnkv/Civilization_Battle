using Common.UnityLogic.Builders.Units;

namespace Common.Infrastructure.Services.SceneContext
{
    public interface ISceneContextService
    {
        GridMap GridMap { get; set; }
        UnitsBuilder UnitsBuilder { get; set; }
    }
}