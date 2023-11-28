using System;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Common.Infrastructure.Services.SceneLoading
{
    public sealed class SceneLoader : ISceneLoader
    {
        public async UniTask LoadSceneAsync(string sceneName, Action onLoaded = null)
        {
            var waitNextScene = SceneManager.LoadSceneAsync(sceneName);
            await waitNextScene;
            onLoaded?.Invoke();
        }
    }
}