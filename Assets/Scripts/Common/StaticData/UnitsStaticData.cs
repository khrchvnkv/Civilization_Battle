using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.StaticData
{
    public sealed class UnitsStaticData
    {
        private readonly Dictionary<string, UnitStaticData> _unitStaticDataMap;

        public UnitsStaticData(in UnitStaticData[] unitStaticDatas) => 
            _unitStaticDataMap = unitStaticDatas.ToDictionary(x => x.UnitName, y => y);

        public UnitStaticData GetStaticDataForUnit(in string unitName)
        {
            if (_unitStaticDataMap.TryGetValue(unitName, out var data))
            {
                return data;
            }

            throw new Exception($"No unit data for unit with name {unitName}");
        }
    }
}