using System.Collections.Generic;
using System.Linq;
using Common.Infrastructure.Factories.Zenject;
using Common.Infrastructure.Services.Input;
using Common.Infrastructure.Services.StaticData;
using Common.UnityLogic.Builders.Grid;
using Common.UnityLogic.Units;
using UnityEngine;

namespace Common.Infrastructure.Factories.UnitsFactory
{
    public sealed class UnitsFactory : IUnitsFactory
    {
        private readonly IZenjectFactory _zenjectFactory;
        private readonly IStaticDataService _staticDataService;
        private readonly IInputService _inputService;
        private readonly HashSet<Unit> _createdUnits;

        public UnitsFactory(IZenjectFactory zenjectFactory, IStaticDataService staticDataService, IInputService inputService)
        {
            _zenjectFactory = zenjectFactory;
            _staticDataService = staticDataService;
            _inputService = inputService;
            _createdUnits = new();
        }

        public Unit SpawnUnit(in string unitName, in TeamTypes teamType, in Cell cell)
        {
            var unitData = _staticDataService.UnitsStaticData.GetStaticDataForUnit(unitName);
            var unit = _zenjectFactory.Instantiate(unitData.Unit, cell.UnitSpawnPoint);
            unit.Init(unitData, teamType, cell.Data);
            _createdUnits.Add(unit);
            return unit;
        }

        public void DespawnUnit(in Unit unit)
        {
            _createdUnits.Remove(unit);
            Object.Destroy(unit.gameObject);
        }

        public void TrySelectAvailableUnit()
        {
            var availableUnit = _createdUnits.FirstOrDefault(x => x.IsAvailable && x.Model.HasAvailableRange);
            if (availableUnit is not null) _inputService.SelectUnit(availableUnit);
        }
    }
}