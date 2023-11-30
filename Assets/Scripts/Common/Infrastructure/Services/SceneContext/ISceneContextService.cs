using Common.UnityLogic.Builders.Units;
using UnityEngine;

namespace Common.Infrastructure.Services.SceneContext
{
    public interface ISceneContextService
    {
        Camera MainCamera { get; set; }
        GridMap GridMap { get; set; }
        UnitsBuilder UnitsBuilder { get; set; }
    }
}