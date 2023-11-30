using Common.StaticData;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Common.Infrastructure.Services.AssetsManagement
{
    public sealed class AssetProvider : IAssetProvider
    {
        private const string GAME_STATIC_DATA_PATH = "StaticData/GameStaticData";

        public GameStaticData LoadGameStaticData() => 
            Load<GameStaticData>(GAME_STATIC_DATA_PATH);

        public UnitStaticData[] LoadUnitsStaticData() => LoadAll<UnitStaticData>(Constants.UnitDataPath.LocalPath);
        public GameObject Load(string path) => Load<GameObject>(path);
        public async UniTask<GameObject> LoadAsync(string path) => await LoadAsync<GameObject>(path);
        private T Load<T>(string path) where T : Object => Resources.Load<T>(path);
        private async UniTask<T> LoadAsync<T>(string path) where T : Object
        {
            var result = await Resources.LoadAsync<T>(path);
            return result as T;
        }
        private T[] LoadAll<T>(string path) where T : Object => Resources.LoadAll<T>(path);
    }
}