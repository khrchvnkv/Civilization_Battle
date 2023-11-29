using Common.Infrastructure.Services.ECS;
using UnityEngine;
using Zenject;

namespace Common.UnityLogic.Ecs.Providers
{
    public abstract class MonoProvider : MonoBehaviour
    {
        protected int EntityID;

        private IEcsStartup _ecsStartup;

        [Inject]
        private void Construct(IEcsStartup ecsStartup) => _ecsStartup = ecsStartup;

        protected virtual void EnableEntity() => EntityID = _ecsStartup.World.NewEntity();
        private void DisableEntity()
        {
            if (_ecsStartup.World.IsAlive()) _ecsStartup.World.DelEntity(EntityID);
        }

        protected virtual void OnEnable() => EnableEntity();
        protected virtual void OnDisable() => DisableEntity();
    }
}