using System.Collections.Generic;
using Common.Infrastructure.Factories.UnitsFactory;
using Common.Infrastructure.Services.ECS;
using Common.UnityLogic.Builders.Grid;
using Common.UnityLogic.Ecs.OneFrames;
using Common.UnityLogic.Units;
using UnityEngine;
using Zenject;

namespace Common.UnityLogic.Builders.Units
{
    [RequireComponent(typeof(GridBuilder))]
    public sealed class UnitsBuilder : MonoBehaviour
    {
        [SerializeField] private GridBuilder _gridBuilder;

        private IUnitsFactory _unitsFactory;
        private IEcsStartup _ecsStartup;
        
        private List<Unit> _units = new();

        public IEnumerable<Unit> Units => _units;
        private IEnumerable<CellBuilder> CellBuilders => _gridBuilder.CellBuilders;

        private void OnValidate() => _gridBuilder ??= gameObject.GetComponent<GridBuilder>();

        [Inject]
        private void Construct(IUnitsFactory unitsFactory, IEcsStartup ecsStartup)
        {
            _unitsFactory = unitsFactory;
            _ecsStartup = ecsStartup;
            
            Init();
        }
        
        private void Init()
        {
            foreach (var cellBuilder in CellBuilders)
            {
                if (cellBuilder.IsNotDefaultType)
                {
                    _units.Add(_unitsFactory.SpawnUnit(cellBuilder.UnitName, cellBuilder.TeamType, cellBuilder.Cell));
                }
            }

            ActivateBattleSystem();
        }

        private void ActivateBattleSystem()
        {
            var entity = _ecsStartup.World.NewEntity();
            _ecsStartup.World.GetPool<EnableBattleSystemEvent>().Add(entity);
        }
    }
}