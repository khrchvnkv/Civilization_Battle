using Leopotam.EcsLite;

namespace Common.Infrastructure.Services.ECS
{
    public interface IEcsStartup
    {
        EcsWorld World { get; }
    }
}