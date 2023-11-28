using UnityEngine;

namespace Common.Infrastructure.Services.DontDestroyOnLoadCreator
{
    public interface IDontDestroyOnLoadCreator
    {
        GameObject MarkAsDontDestroy(GameObject instance);
    }
}