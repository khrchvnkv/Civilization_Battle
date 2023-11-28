using UnityEngine;

namespace Common.StaticData
{
    [CreateAssetMenu(fileName = "GameStaticData", menuName = "Static Data/GameStaticData")]
    public class GameStaticData : ScriptableObject
    {
        [field: SerializeField] public WindowStaticData WindowStaticData { get; private set; }
    }
}