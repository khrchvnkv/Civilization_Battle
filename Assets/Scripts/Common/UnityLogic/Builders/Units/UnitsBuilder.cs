using System.Collections.Generic;
using Common.Infrastructure.Factories.UnitsFactory;
using Common.UnityLogic.Builders.Grid;
using UnityEngine;
using Zenject;

namespace Common.UnityLogic.Builders.Units
{
    [RequireComponent(typeof(GridBuilder))]
    public sealed class UnitsBuilder : MonoBehaviour
    {
        [SerializeField] private GridBuilder _gridBuilder;

        private IUnitsFactory _unitsFactory;

        private IEnumerable<Cell> Cells => _gridBuilder.Cells;

        private void OnValidate() => _gridBuilder ??= gameObject.GetComponent<GridBuilder>();

        [Inject]
        private void Construct(IUnitsFactory unitsFactory)
        {
            _unitsFactory = unitsFactory;
            
            Init();
        }
        
        private void Init()
        {
            foreach (var cell in Cells)
            {
                if (cell.IsNotDefaultType)
                {
                    _unitsFactory.SpawnUnit(cell.UnitName, cell.UnitSpawnPoint);
                }
            }
        }
    }
}