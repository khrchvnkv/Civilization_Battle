using System;
using System.Linq;
using Common.StaticData;
using Common.UnityLogic.Units;
using NaughtyAttributes;
using UnityEngine;

namespace Common.UnityLogic.Builders.Grid
{
    public sealed class Cell : MonoBehaviour
    {
        private const string DefaultType = "NONE";

        [field: SerializeField] public Transform UnitSpawnPoint { get; private set; }
        [field: SerializeField] public Vector2Int Data { get; set; }
        [field: Dropdown(nameof(GetUnitNames)), SerializeField] public string UnitName { get; private set; }
        [field: ShowIf(nameof(IsNotDefaultType)), SerializeField] public TeamTypes TeamType { get; private set; }
        
        public bool IsNotDefaultType => UnitName != DefaultType;
        
        private void OnDrawGizmos()
        {
            if (string.IsNullOrWhiteSpace(UnitName) || !IsNotDefaultType) return;

            Gizmos.color = Constants.TeamColors[TeamType];
            Gizmos.DrawSphere(UnitSpawnPoint.position, 1.0f);
        }

        private DropdownList<string> GetUnitNames()
        {
            var datas = Resources.LoadAll<UnitStaticData>(Constants.UnitDataPath.LocalPath);
            var names = datas.Select(x => x.UnitName);
            var result = new DropdownList<string>();
            result.Add(DefaultType, DefaultType);
            foreach (var name in names)
            {
                result.Add(name, name);
            }

            return result;
        }
    }
}