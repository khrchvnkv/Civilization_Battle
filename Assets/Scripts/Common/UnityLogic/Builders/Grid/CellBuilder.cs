using System.Linq;
using Common.StaticData;
using Common.UnityLogic.Units;
using NaughtyAttributes;
using UnityEngine;

namespace Common.UnityLogic.Builders.Grid
{
    [RequireComponent(typeof(Cell))]
    public sealed class CellBuilder : MonoBehaviour
    {
        private const string DefaultType = "NONE";

        [field: SerializeField] public Cell Cell { get; private set; }
        [field: Dropdown(nameof(GetUnitNames)), SerializeField] public string UnitName { get; private set; }
        [field: ShowIf(nameof(IsNotDefaultType)), SerializeField] public TeamTypes TeamType { get; private set; }
        
        public bool IsNotDefaultType => UnitName != DefaultType;

        private void OnValidate() => Cell ??= gameObject.GetComponent<Cell>();

        private void OnDrawGizmos()
        {
            if (string.IsNullOrWhiteSpace(UnitName) || !IsNotDefaultType) return;

            Gizmos.color = Constants.TeamColors[TeamType];
            Gizmos.DrawSphere(transform.position, 1.0f);
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