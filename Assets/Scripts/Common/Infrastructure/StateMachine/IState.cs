namespace Common.Infrastructure.StateMachine
{
    public interface IState : IExitableState
    {
        void Enter();
    }
}