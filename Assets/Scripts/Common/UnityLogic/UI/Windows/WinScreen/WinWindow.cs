using System;
using Common.Infrastructure.Factories.UIFactory;
using Common.Infrastructure.Services.Input;
using Common.Infrastructure.Services.SceneContext;
using Common.UnityLogic.Units;
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
        private IInputService _inputService;

        [Inject]
        private void Construct(ISceneContextService sceneContextService, IUIFactory uiFactory, IInputService inputService)
        {
            _sceneContextService = sceneContextService;
            _uiFactory = uiFactory;
            _inputService = inputService;
        }

        protected override void PrepareForShowing()
        {
            base.PrepareForShowing();
            
            _inputService.Disable();
            
            _winText.text = $"{Enum.GetName(typeof(TeamTypes), WindowData.WinningTeam)} WON!";
            _winText.color = Constants.TeamColors[WindowData.WinningTeam];
            
            _restartButton.onClick.AddListener(Restart);
        }

        protected override void PrepareForHiding()
        {
            base.PrepareForHiding();
            
            _restartButton.onClick.RemoveListener(Restart);
        }

        private void Restart()
        {
            _sceneContextService.ResetScene();
            _uiFactory.Hide(new WinWindowData());
        }
    }
}