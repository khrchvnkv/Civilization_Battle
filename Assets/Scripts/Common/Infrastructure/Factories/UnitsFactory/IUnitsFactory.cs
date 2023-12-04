using Common.UnityLogic.Builders.Grid;
using Common.UnityLogic.Units;

namespace Common.Infrastructure.Factories.UnitsFactory
{
    public interface IUnitsFactory
    {
        public Unit SpawnUnit(in string unitName, in TeamTypes teamType, in Cell cell);
        void DespawnUnit(in Unit unit);
        void TrySelectAvailableUnit();
    }
}