using Common.Infrastructure.Services.ECS;
using Leopotam.EcsLite;
using UnityEngine;
using Zenject;

namespace Common.UnityLogic.Ecs.Providers
{
    public abstract class MonoProvider : MonoBehaviour
    {
        private IEcsStartup _ecsStartup;

        public int EntityID { get; private set; }
        protected EcsWorld World => _ecsStartup.World;

        [Inject]
        private void Construct(IEcsStartup ecsStartup) => _ecsStartup = ecsStartup;

        protected virtual void EnableEntity() => EntityID = World.NewEntity();
        protected virtual void DisableEntity()
        {
            if (World.IsAlive()) World.DelEntity(EntityID);
        }

        protected virtual void OnEnable() => EnableEntity();
        protected virtual void OnDisable() => DisableEntity();

        protected void AddComponent<T>(in T componentInstance) where T : struct
        {
            ref var component = ref World.GetPool<T>().Add(EntityID);
            component = componentInstance;
        }
    }
}