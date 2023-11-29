using System.Collections.Generic;
using Common.UnityLogic.Units;
using UnityEngine;

namespace Common
{
    public static class Constants
    {
        public static class Scenes
        {
            public const string BootstrapScene = "Bootstrap";
            public const string GameScene = "GameScene";
        }

        public static class UnitDataPath
        {
            public const string GlobalPath = "Assets/Resources";
            public const string LocalPath = "StaticData/UnitsData";
        }

        public static Dictionary<TeamTypes, Color> TeamColors = new()
        {
            { TeamTypes.TeamA, Color.blue },
            { TeamTypes.TeamB, Color.red },
        };
    }
}