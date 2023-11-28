using Common.StaticData;
using UnityEngine;

namespace Common.Infrastructure.Services.AssetsManagement
{
    public sealed class AssetProvider : IAssetProvider
    {
        private const string GAME_STATIC_DATA_PATH = "StaticData/GameStaticData";

        public GameStaticData LoadGameStaticData() => Load<GameStaticData>(GAME_STATIC_DATA_PATH);
        public GameObject Load(in string path) => Load<GameObject>(path);
        private T Load<T>(in string path) where T : Object => Resources.Load<T>(path);
    }
}