using Leopotam.EcsLite;

namespace Common.Extensions
{
    public static class EcsExtensions
    {
        public static void AddComponent<T>(this EcsWorld world, in int entityId, ref T component) where T : struct
        {
            var pool = world.GetPool<T>();
            pool.Add(entityId);

            ref var ecsComponent = ref pool.Get(entityId);
            ecsComponent = component;
        }
    }
}