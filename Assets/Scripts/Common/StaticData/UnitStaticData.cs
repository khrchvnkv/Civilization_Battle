using Common.UnityLogic.Units;
using UnityEngine;

namespace Common.StaticData
{
    [CreateAssetMenu(fileName = "UnitData", menuName = "Static Data/Units/Data")]
    public sealed class UnitStaticData : ScriptableObject
    {
        [field: SerializeField] public string UnitName { get; set; }
        [field: SerializeField] public float HP { get; set; }
        [field: SerializeField] public float Damage { get; set; }
        [field: SerializeField] public float DiagonalAttackMultiplier { get; set; }
        [field: SerializeField] public int Range { get; set; }
        [field: SerializeField] public Unit Unit { get; set; }
    }
}