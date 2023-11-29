using Common.Infrastructure.Factories.Zenject;
using Common.Infrastructure.Services.StaticData;
using Common.UnityLogic.Units;
using UnityEngine;

namespace Common.Infrastructure.Factories.UnitsFactory
{
    public sealed class UnitsFactory : IUnitsFactory
    {
        private readonly IZenjectFactory _zenjectFactory;
        private readonly IStaticDataService _staticDataService;

        public UnitsFactory(IZenjectFactory zenjectFactory, IStaticDataService staticDataService)
        {
            _zenjectFactory = zenjectFactory;
            _staticDataService = staticDataService;
        }
        
        public Unit SpawnUnit(in string unitName, in Transform spawnPoint)
        {
            var unitData = _staticDataService.UnitsStaticData.GetStaticDataForUnit(unitName);
            var unit = _zenjectFactory.Instantiate(unitData.Unit, spawnPoint);
            return unit;
        }

        public void DespawnUnit(in Unit unit) => Object.Destroy(unit.gameObject);
    }
}