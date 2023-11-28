using UnityEngine;

namespace Common.Extensions
{
    public static class Utils
    {
        public static string Serialize(this object obj) => JsonUtility.ToJson(obj);
        
        public static T Deserialize<T>(this string json) => JsonUtility.FromJson<T>(json);
    }
}