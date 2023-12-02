using System;
using DG.Tweening;
using UnityEngine;

namespace Common.UnityLogic.UI.LoadingScreen
{
    public class LoadingCurtain : MonoBehaviour
    {
        private const float FADE_DURATION = 1.5f;

        [SerializeField] private CanvasGroup _canvasGroup;

        private Tween _tween;
        
        public void Show()
        {
            KillTween();
            _canvasGroup.alpha = 1.0f;
            _canvasGroup.gameObject.SetActive(true);
        }
        public void Hide()
        {
            KillTween();
            _tween = _canvasGroup.DOFade(0.0f, FADE_DURATION)
                .OnComplete(() => _canvasGroup.gameObject.SetActive(false));
        }

        private void OnDestroy() => KillTween();
        private void KillTween() => _tween?.Kill();
    }
}