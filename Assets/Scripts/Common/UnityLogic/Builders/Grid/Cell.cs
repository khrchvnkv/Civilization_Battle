using UnityEngine;

namespace Common.UnityLogic.Builders.Grid
{
    [RequireComponent(typeof(CellView))]
    public sealed class Cell : MonoBehaviour
    {
        [field: SerializeField] public CellView View { get; private set; }
        [field: SerializeField] public Transform UnitSpawnPoint { get; private set; }
        [field: SerializeField] public Vector2Int Data { get; set; }

        private void OnValidate() => View ??= gameObject.GetComponent<CellView>();
    }
}