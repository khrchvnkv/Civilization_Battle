using UnityEngine;

namespace Common.UnityLogic.Units.View
{
    public sealed class UnitView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _circleSpriteRenderer;
        [SerializeField] private GameObject _availableRangeView;
        [SerializeField] private ParticleSystem _selectedUnitParticle;

        public void UpdateView(in TeamTypes teamType) => _circleSpriteRenderer.color = Constants.TeamColors[teamType];
        public void SetAvailableRangeActivity(in bool isActive) => _availableRangeView.SetActive(isActive);
        public void SetSelectedViewActivity(in bool isActive) => _selectedUnitParticle.gameObject.SetActive(isActive);
    }
}