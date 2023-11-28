using UnityEngine;

namespace Common.StaticData
{
    [CreateAssetMenu(fileName = "UnitData", menuName = "Static Data/Units/Data")]
    public sealed class UnitStaticData : ScriptableObject
    {
        [field: SerializeField] public string UnitName { get; set; }
        [field: SerializeField] public int HP { get; set; }
        [field: SerializeField] public float Damage { get; set; }
        [field: SerializeField] public int Range { get; set; }
    }
}