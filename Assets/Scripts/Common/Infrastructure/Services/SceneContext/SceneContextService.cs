using Common.UnityLogic.Builders.Units;
using UnityEngine;

namespace Common.Infrastructure.Services.SceneContext
{
    public sealed class SceneContextService : ISceneContextService
    {
        public Camera MainCamera { get; set; }
        public GridMap GridMap { get; set; }
        public UnitsBuilder UnitsBuilder { get; set; }
    }
}