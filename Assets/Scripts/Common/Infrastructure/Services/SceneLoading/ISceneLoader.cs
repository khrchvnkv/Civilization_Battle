using System;
using Cysharp.Threading.Tasks;

namespace Common.Infrastructure.Services.SceneLoading
{
    public interface ISceneLoader
    {
        UniTask LoadSceneAsync(string sceneName, Action onLoaded = null);
    }
}