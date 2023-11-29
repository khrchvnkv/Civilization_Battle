using UnityEngine;

namespace Common.UnityLogic.GridLogic
{
    public sealed class Cell : MonoBehaviour
    {
        [field: SerializeField] public float CellSize { get; private set; }
    }
}