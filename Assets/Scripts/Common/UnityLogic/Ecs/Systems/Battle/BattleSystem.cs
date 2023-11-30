using Leopotam.EcsLite;

namespace Common.UnityLogic.Ecs.Systems.Battle
{
    /// <summary>
    /// Смена и завершение хода
    /// </summary>
    public sealed class BattleSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _teamsFilter;
        //private EcsPool<TeamComponent>

        public void Init(IEcsSystems systems)
        {
        }

        public void Run(IEcsSystems systems)
        {
        }
    }
}