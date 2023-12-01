using System.Collections.Generic;
using Common.Infrastructure.Factories.UnitsFactory;
using Common.UnityLogic.Builders.Grid;
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
        private List<Unit> _units = new();

        public IEnumerable<Unit> Units => _units;
        private IEnumerable<CellBuilder> CellBuilders => _gridBuilder.CellBuilders;

        private void OnValidate() => _gridBuilder ??= gameObject.GetComponent<GridBuilder>();

        [Inject]
        private void Construct(IUnitsFactory unitsFactory)
        {
            _unitsFactory = unitsFactory;
            
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
        }
    }
}