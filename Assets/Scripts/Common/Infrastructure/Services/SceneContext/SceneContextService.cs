using Common.Infrastructure.Factories.UIFactory;
using Common.UnityLogic.Builders.Units;
using Common.UnityLogic.UI.Windows.GameHUD;
using UnityEngine;

namespace Common.Infrastructure.Services.SceneContext
{
    public sealed class SceneContextService : ISceneContextService
    {
        private readonly IUIFactory _uiFactory;
        
        public Camera MainCamera { get; set; }
        public GridMap GridMap { get; set; }
        public UnitsBuilder UnitsBuilder { get; set; }

        public SceneContextService(IUIFactory uiFactory) => _uiFactory = uiFactory;

        public void ResetScene()
        {
            _uiFactory.ShowLoadingCurtain();
            
            GridMap.HidePath();
            UnitsBuilder.ResetUnits();
                
            _uiFactory.HideLoadingCurtain();
            _uiFactory.ShowWindow(new GameHudWindowData());
        }
    }
}