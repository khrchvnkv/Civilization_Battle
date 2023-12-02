using System;
using DG.Tweening;
using UnityEngine;

namespace Common.UnityLogic.Builders.Grid
{
    public sealed class CellView : MonoBehaviour
    {
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private Transform _visualTransform;

        [Header("Visual")] 
        [SerializeField] private Material _defaultMaterial;
        [SerializeField] private Material _availableMaterial;
        [SerializeField] private Material _readyAttackMaterial;

        private Tween _tween;
        
        private void OnValidate()
        {
            if (_meshRenderer is not null) _visualTransform ??= _meshRenderer.transform;
        }

        private void OnDestroy() => _tween?.Kill();

        public void ShowAvailableNode() => _meshRenderer.sharedMaterial = _availableMaterial;
        
        public void ShowAttackableNode() => _meshRenderer.sharedMaterial = _readyAttackMaterial;

        public void HideAvailableNode() => _meshRenderer.sharedMaterial = _defaultMaterial;

        public void SetHovered() => _tween = _visualTransform.DOMoveY(0.3f, 0.5f);

        public void SetUnhovered() => _tween = _visualTransform.DOMoveY(0.0f, 0.5f);
    }
}