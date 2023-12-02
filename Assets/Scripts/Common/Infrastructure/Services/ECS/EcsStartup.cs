using System;
using Common.Infrastructure.Services.MonoUpdate;
using Common.UnityLogic.Ecs.OneFrames;
using Common.UnityLogic.Ecs.Systems.Battle;
using Common.UnityLogic.Ecs.Systems.Battle.UnitMovement;
using Common.UnityLogic.Ecs.Systems.Battle.UnitSelection;
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
        
        public void Dispose()
        {
            _monoUpdateSystem.OnUpdate -= UpdateEcs;
            _updateSystems.Destroy();
            World.Destroy();
        }
        
        public void EnableBattleSystem()
        {
            var entity = World.NewEntity();
            World.GetPool<EnableBattleSystemEvent>().Add(entity);
        }

        public void DisableBattleSystem()
        {
            var entity = World.NewEntity();
            World.GetPool<DisableBattleSystemEvent>().Add(entity);
        }

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
            AddSystem<UnitSelectionSystem>();
            AddSystem<UnitMovementSystem>();
            AddSystem<BattleSystem>();
        }

        private void AddSystem<T>() where T : IEcsSystem, new()
        {
            var system = new T();
            _diContainer.Inject(system);
            _updateSystems.Add(system);
        }

        private void UpdateEcs() => _updateSystems?.Run();
    }
}