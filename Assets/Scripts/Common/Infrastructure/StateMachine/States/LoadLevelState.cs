using Common.Infrastructure.Factories.UIFactory;
using Common.Infrastructure.Services.SceneLoading;
using Cysharp.Threading.Tasks;

namespace Common.Infrastructure.StateMachine.States
{
    public class LoadLevelState : State, IState
    {
        private readonly ISceneLoader _sceneLoader;
        private readonly IUIFactory _uiFactory;

        public LoadLevelState(ISceneLoader sceneLoader, IUIFactory uiFactory)
        {
            _sceneLoader = sceneLoader;
            _uiFactory = uiFactory;
        }

        public void Enter() => 
            _sceneLoader.LoadSceneAsync(Constants.Scenes.GameScene, OnGameSceneLoaded).Forget();
        public override void Exit() => _uiFactory.HideLoadingCurtain();
        private void OnGameSceneLoaded() => StateMachine.Enter<GameLoopState>();
    }
}