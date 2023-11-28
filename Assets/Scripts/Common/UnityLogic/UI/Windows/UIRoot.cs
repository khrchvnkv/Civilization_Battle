using Common.UnityLogic.UI.LoadingScreen;
using UnityEngine;

namespace Common.UnityLogic.UI.Windows
{
    public sealed class UIRoot : MonoBehaviour
    {
        [field: SerializeField] public Transform WindowsParent { get; private set; }
        [field: SerializeField] public LoadingCurtain LoadingCurtain { get; set; }
    }
}