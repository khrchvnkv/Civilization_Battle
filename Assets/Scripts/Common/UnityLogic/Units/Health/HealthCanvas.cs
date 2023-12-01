using UnityEngine;
using UnityEngine.UI;

namespace Common.UnityLogic.Units.Health
{
    public sealed class HealthCanvas : MonoBehaviour
    {
        [SerializeField] private Image _slider;

        public void UpdateHP(float percent) => _slider.fillAmount = percent;
        public void Show() => gameObject.SetActive(true);
        public void Hide() => gameObject.SetActive(false);
    }
}