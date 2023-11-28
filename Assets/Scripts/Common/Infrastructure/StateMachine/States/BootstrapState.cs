using Common.Infrastructure.Factories.UIFactory;
using Common.Infrastructure.Services.Progress;
using Common.Infrastructure.Services.SavedData;
using Common.Infrastructure.Services.SaveLoad;
using Common.Infrastructure.Services.StaticData;

namespace Common.Infrastructure.StateMachine.States
{
    /// <summary>
    /// Data loading, UIRoot creating
    /// </summary>
    public class BootstrapState : State, IState
    {
        private readonly IPersistentProgressService _persistentProgressService;
        private readonly ISaveLoadService _saveLoadService;
        private readonly IStaticDataService _staticDataService;
        private readonly IUIFactory _uiFactory;

        public BootstrapState(IPersistentProgressService persistentProgressService, 
            ISaveLoadService saveLoadService, IStaticDataService staticDataService, 
            IUIFactory uiFactory)
        {
            _persistentProgressService = persistentProgressService;
            _saveLoadService = saveLoadService;
            _staticDataService = staticDataService;
            _uiFactory = uiFactory;
        }
        public void Enter()
        {
            LoadProgressOrCreateNew();
            LoadStaticData();
            CreateUIRootAndShowLoadingCurtain();
            
            StateMachine.Enter<LoadLevelState>();
        }
        public override void Exit()
        { }
        private void LoadProgressOrCreateNew()
        {
            _persistentProgressService.SaveData = _saveLoadService.LoadData() ?? new SaveData();
            _saveLoadService.SaveData();
        }
        private void LoadStaticData() => _staticDataService.Load();
        private void CreateUIRootAndShowLoadingCurtain()
        {
            _uiFactory.CreateUIRoot();
            _uiFactory.ShowLoadingCurtain();
        }
    }
}