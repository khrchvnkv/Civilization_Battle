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
        
        private void OnValidate()
        {
            if (_meshRenderer is not null) _visualTransform ??= _meshRenderer.transform;
        }
        
        public void ShowAvailablePath() => _meshRenderer.sharedMaterial = _availableMaterial;

        public void HideAvailablePath() => _meshRenderer.sharedMaterial = _defaultMaterial;

        public void SetHovered() => _visualTransform.DOMoveY(0.3f, 0.5f);

        public void SetUnhovered() => _visualTransform.DOMoveY(0.0f, 0.5f);
    }
}