using System;

namespace Common.Infrastructure.Services.MonoUpdate
{
    public interface IMonoUpdateSystem
    {
        event Action OnUpdate;
        event Action OnFixedUpdate;
        event Action OnLateUpdate;
    }
}