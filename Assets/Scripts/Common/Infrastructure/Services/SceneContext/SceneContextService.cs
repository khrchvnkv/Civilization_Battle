using Common.UnityLogic.Builders.Units;

namespace Common.Infrastructure.Services.SceneContext
{
    public sealed class SceneContextService : ISceneContextService
    {
        public GridMap GridMap { get; set; }
        public UnitsBuilder UnitsBuilder { get; set; }
    }
}