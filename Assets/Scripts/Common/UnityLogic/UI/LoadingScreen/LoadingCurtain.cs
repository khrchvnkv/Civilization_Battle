using System.Collections;
using Common.Infrastructure.Services.Coroutines;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace Common.UnityLogic.UI.LoadingScreen
{
    public class LoadingCurtain : MonoBehaviour
    {
        private const float FADE_DURATION = 1.5f;

        [SerializeField] private CanvasGroup _canvasGroup;
        
        public void Show()
        {
            DOTween.Kill(_canvasGroup);
            _canvasGroup.alpha = 1.0f;
            _canvasGroup.gameObject.SetActive(true);
        }
        public void Hide()
        {
            DOTween.Kill(_canvasGroup);
            DOTween.To(() => _canvasGroup.alpha, x => _canvasGroup.alpha = x, 0.0f, FADE_DURATION)
                .OnComplete(() => _canvasGroup.gameObject.SetActive(false));
        }
    }
}