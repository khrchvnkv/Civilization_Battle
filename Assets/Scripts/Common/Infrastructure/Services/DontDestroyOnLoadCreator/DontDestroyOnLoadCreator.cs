using UnityEngine;

namespace Common.Infrastructure.Services.DontDestroyOnLoadCreator
{
    public sealed class DontDestroyOnLoadCreator : MonoBehaviour, IDontDestroyOnLoadCreator
    {
        public GameObject MarkAsDontDestroy(GameObject instance)
        {
            DontDestroyOnLoad(instance);
            return instance;
        }
    }
}