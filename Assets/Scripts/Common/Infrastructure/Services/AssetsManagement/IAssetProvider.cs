using Common.StaticData;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Common.Infrastructure.Services.AssetsManagement
{
    public interface IAssetProvider
    {
        GameStaticData LoadGameStaticData();
        UnitStaticData[] LoadUnitsStaticData();

        GameObject Load(string path);
        UniTask<GameObject> LoadAsync(string path);
    }
}