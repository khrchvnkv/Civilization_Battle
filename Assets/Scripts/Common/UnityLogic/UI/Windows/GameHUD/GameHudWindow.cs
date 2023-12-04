using System.Text;
using Common.Infrastructure.Services.ECS;
using Common.Infrastructure.Services.Input;
using Common.Infrastructure.Services.SceneContext;
using Common.UnityLogic.Ecs.OneFrames;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Common.UnityLogic.UI.Windows.GameHUD
{
    public sealed class GameHudWindow : WindowBase<GameHudWindowData>
    {
        [SerializeField] private Button _nextTurnButton;
        [SerializeField] private Button _restartButton;
        [SerializeField] private GameObject _unitSlot;
        [SerializeField] private TMP_Text _statsText;

        private IEcsStartup _ecsStartup;
        private ISceneContextService _sceneContextService;

        [Inject]
        private void Construct(IEcsStartup ecsStartup, ISceneContextService sceneContextService)
        {
            _ecsStartup = ecsStartup;
            _sceneContextService = sceneContextService;
        }

        protected override void PrepareForShowing()
        {
            base.PrepareForShowing();

            _nextTurnButton.onClick.AddListener(NextTurn);
            _restartButton.onClick.AddListener(Restart);

            var unitSlotActive = WindowData.Unit.HasValue;
            _unitSlot.SetActive(unitSlotActive);
            if (unitSlotActive)
            {
                var unitModel = WindowData.Unit.Value.UnitModel;

                var sb = new StringBuilder();
                sb.Append($"HP: {unitModel.HP}/{unitModel.StaticData.HP}\n");
                sb.Append($"Damage: {unitModel.StaticData.Damage}\n");
                sb.Append($"Movement: {unitModel.AvailableMovementRange}/{unitModel.StaticData.Range}");
                _statsText.text = sb.ToString();
            }
        }

        protected override void PrepareForHiding()
        {
            base.PrepareForHiding();
            
            _nextTurnButton.onClick.RemoveListener(NextTurn);
            _restartButton.onClick.RemoveListener(Restart);
            
            _ecsStartup.DisableBattleSystem();
        }

        private void NextTurn()
        {
            var entity = _ecsStartup.World.NewEntity();
            _ecsStartup.World.GetPool<NextTurnEvent>().Add(entity);
        }

        private void Restart() => _sceneContextService.ResetScene();
    }
}