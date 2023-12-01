using UnityEngine;

namespace Common.UnityLogic.Units.View
{
    public sealed class UnitView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _circleSpriteRenderer;

        public void UpdateView(in TeamTypes teamType) => _circleSpriteRenderer.color = Constants.TeamColors[teamType];
    }
}