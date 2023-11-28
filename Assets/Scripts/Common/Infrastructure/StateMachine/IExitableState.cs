namespace Common.Infrastructure.StateMachine
{
    public interface IExitableState
    {
        void SetupStateMachine(GameStateMachine stateMachine);
        void Exit();
    }
}