using Common.UnityLogic.Units;
using UnityEngine;

namespace Common.Infrastructure.Factories.UnitsFactory
{
    public interface IUnitsFactory
    {
        Unit SpawnUnit(in string unitName, in Transform spawnPoint);
        void DespawnUnit(in Unit unit);
    }
}