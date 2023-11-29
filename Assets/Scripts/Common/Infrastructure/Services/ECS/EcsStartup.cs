using System;
using Common.Infrastructure.Services.MonoUpdate;
using Leopotam.EcsLite;
using UnityEngine;
using Zenject;

namespace Common.Infrastructure.Services.ECS
{
    public sealed class EcsStartup : MonoBehaviour, IEcsStartup, IDisposable
    {
        private DiContainer _diContainer;
        private IMonoUpdateSystem _monoUpdateSystem;
        
        private EcsSystems _updateSystems;
        
        public EcsWorld World { get; private set; }

        [Inject]
        private void Construct(DiContainer container, IMonoUpdateSystem monoUpdateSystem)
        {
            _diContainer = container;
            _monoUpdateSystem = monoUpdateSystem;
            
            Init();
        }
        
        public void Dispose() => _monoUpdateSystem.OnUpdate -= UpdateEcs;

        private void Init()
        {
            World = new EcsWorld();

            _updateSystems = new EcsSystems(World);

            AddSystems();
            
            _updateSystems.Init();

            _monoUpdateSystem.OnUpdate += UpdateEcs;
        }

        private void AddSystems()
        {
            
        }

        private void UpdateEcs() => _updateSystems?.Run();

    }
}