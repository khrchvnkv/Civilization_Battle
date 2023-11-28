using Common.UnityLogic.UI.LoadingScreen;
using Common.UnityLogic.UI.Windows;
using UnityEngine;

namespace Common.StaticData
{
    [CreateAssetMenu(menuName = nameof(WindowStaticData), fileName = nameof(WindowStaticData), order = 0)]
    public sealed class WindowStaticData : ScriptableObject
    {
        [field: SerializeField] public UIRoot UIRoot { get; private set; }
        [field: SerializeField] public LoadingCurtain LoadingCurtainPrefab { get; private set; }
    }
}