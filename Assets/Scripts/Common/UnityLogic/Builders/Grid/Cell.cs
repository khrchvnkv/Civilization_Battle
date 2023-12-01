using UnityEngine;

namespace Common.UnityLogic.Builders.Grid
{
    [RequireComponent(typeof(CellView))]
    public sealed class Cell : MonoBehaviour
    {
        [SerializeField] private CellView _cellView;
        [field: SerializeField] public Transform UnitSpawnPoint { get; private set; }
        [field: SerializeField] public Vector2Int Data { get; set; }

        private void OnValidate() => _cellView ??= gameObject.GetComponent<CellView>();

        public void ShowAvailablePath() => _cellView.ShowAvailablePath();
        public void HideAvailablePath() => _cellView.HideAvailablePath();
        public void Hover() => _cellView.SetHovered();
        public void Unhover() => _cellView.SetUnhovered();
    }
}