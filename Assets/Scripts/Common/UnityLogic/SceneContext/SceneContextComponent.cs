using Common.Infrastructure.Factories.UIFactory;
using Common.Infrastructure.Services.SceneContext;
using Common.UnityLogic.Builders.Grid;
using Common.UnityLogic.Builders.Units;
using Common.UnityLogic.UI.Windows.GameHUD;
using UnityEngine;
using Zenject;

namespace Common.UnityLogic.SceneContext
{
    public sealed class SceneContextComponent : MonoBehaviour
    {
        [SerializeField] private GridBuilder _gridBuilder;
        [SerializeField] private UnitsBuilder _unitsBuilder;

        private ISceneContextService _sceneContextService;
        private IUIFactory _uiFactory;
        
        [Inject]
        private void Construct(ISceneContextService sceneContextService, IUIFactory uiFactory)
        {
            _sceneContextService = sceneContextService;
            _uiFactory = uiFactory;
            
            Init();
        }

        private void Init()
        {
            _sceneContextService.MainCamera = Camera.main;
            _sceneContextService.UnitsBuilder = _unitsBuilder;
            _sceneContextService.GridMap = new GridMap(_unitsBuilder, _gridBuilder.CellBuilders);

            _uiFactory.ShowWindow(new GameHudWindowData());
        }
    }
}