using Common.Infrastructure;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Common.UnityLogic
{
    public sealed class GameRunner : MonoBehaviour
    {
#if UNITY_EDITOR
        private void Awake()
        {
            var currentScene = SceneManager.GetActiveScene().name;
            const string bootstrapScene = Constants.Scenes.BootstrapScene;

            if (currentScene != bootstrapScene)
            {
                var bootstrapper = FindObjectOfType<GameBootstrapper>();
                if (bootstrapper is null)
                {
                    SceneManager.LoadScene(bootstrapScene);
                }
            }
        }
#endif
    }
}