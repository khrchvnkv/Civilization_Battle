using UnityEngine;

namespace Common.UnityLogic.UI.Windows.GameHUD
{
    public sealed class GameHudWindow : WindowBase<GameHudWindowData>
    {
        [SerializeField] private GameObject _unitSlot;
        
        protected override void PrepareForShowing()
        {
            base.PrepareForShowing();
            
            _unitSlot.SetActive(WindowData.Unit.HasValue);
        }
    }
}