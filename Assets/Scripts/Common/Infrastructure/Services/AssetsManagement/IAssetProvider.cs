using Common.StaticData;
using UnityEngine;

namespace Common.Infrastructure.Services.AssetsManagement
{
    public interface IAssetProvider
    {
        GameStaticData LoadGameStaticData();
        GameObject Load(in string path);
    }
}