using System;
using Common.Infrastructure.Factories.UIFactory;
using Common.Infrastructure.Services.Input;
using Common.Infrastructure.Services.SceneContext;
using Common.UnityLogic.Units;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Common.UnityLogic.UI.Windows.WinScreen
{
    public sealed class WinWindow : WindowBase<WinWindowData>
    {
        [SerializeField] private TMP_Text _winText;
        [SerializeField] private Button _restartButton;

        private ISceneContextService _sceneContextService;
        private IUIFactory _uiFactory;
        private IUnitsControlService _unitsControlService;

        [Inject]
        private void Construct(ISceneContextService sceneContextService, IUIFactory uiFactory, 
            IUnitsControlService unitsControlService)
        {
            _sceneContextService = sceneContextService;
            _uiFactory = uiFactory;
            _unitsControlService = unitsControlService;
        }

        protected override void PrepareForShowing()
        {
            base.PrepareForShowing();
            
            _unitsControlService.Disable();
            
            _winText.text = $"{Enum.GetName(typeof(TeamTypes), WindowData.WinningTeam)} WON!";
            _winText.color = Constants.TeamColors[WindowData.WinningTeam];
            
            _restartButton.onClick.AddListener(Restart);
        }

        protected override void PrepareForHiding()
        {
            base.PrepareForHiding();
            
            _unitsControlService.Enable();
            _restartButton.onClick.RemoveListener(Restart);
        }

        private void Restart()
        {
            _uiFactory.Hide(new WinWindowData());
            _sceneContextService.ResetScene();
        }
    }
}