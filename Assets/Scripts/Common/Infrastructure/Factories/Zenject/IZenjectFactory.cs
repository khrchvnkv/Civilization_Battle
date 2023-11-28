using UnityEngine;

namespace Common.Infrastructure.Factories.Zenject
{
    public interface IZenjectFactory
    {
        GameObject Instantiate(GameObject gameObject);
        GameObject Instantiate(GameObject gameObject, Transform parent);
        T Instantiate<T>(T behaviour) where T : MonoBehaviour;
        T Instantiate<T>(T behaviour, Transform parent) where T : MonoBehaviour;
        T Instantiate<T>(T behaviour, Vector3 position, Transform parent = null) where T : MonoBehaviour;
        T Instantiate<T>(T behaviour, Vector3 position, Quaternion quaternion, Transform parent = null) where T : MonoBehaviour;
    }
}