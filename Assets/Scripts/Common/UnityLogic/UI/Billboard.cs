using Common.Infrastructure.Services.MonoUpdate;
using Common.Infrastructure.Services.SceneContext;
using UnityEngine;
using Zenject;

namespace Common.UnityLogic.UI
{
    public sealed class Billboard : MonoBehaviour
    {
        [SerializeField] private Transform _transform;

        private IMonoUpdateSystem _monoUpdateSystem;
        private Transform _mainCameraTransform;
        
        private void OnValidate() => _transform ??= transform;
        
        [Inject]
        private void Construct(IMonoUpdateSystem monoUpdateSystem, ISceneContextService sceneContextService)
        {
            _monoUpdateSystem = monoUpdateSystem;
            _mainCameraTransform = sceneContextService.MainCamera.transform;
        }

        private void OnEnable() => _monoUpdateSystem.OnLateUpdate += UpdateBillboard;

        private void OnDisable() => _monoUpdateSystem.OnLateUpdate -= UpdateBillboard;

        private void UpdateBillboard() =>
            _transform.rotation = _mainCameraTransform.rotation;
    }
}