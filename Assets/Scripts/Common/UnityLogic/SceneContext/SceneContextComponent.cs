using Common.Infrastructure.Services.SceneContext;
using Common.UnityLogic.Builders.Grid;
using Common.UnityLogic.Builders.Units;
using UnityEngine;
using Zenject;

namespace Common.UnityLogic.SceneContext
{
    public sealed class SceneContextComponent : MonoBehaviour
    {
        [SerializeField] private GridBuilder _gridBuilder;
        [SerializeField] private UnitsBuilder _unitsBuilder;

        private ISceneContextService _sceneContextService;
        
        [Inject]
        private void Construct(ISceneContextService sceneContextService)
        {
            _sceneContextService = sceneContextService;

            Init();
        }

        private void Init()
        {
            _sceneContextService.GridMap = new GridMap(_gridBuilder.Cells);
            _sceneContextService.UnitsBuilder = _unitsBuilder;
        }
    }
}