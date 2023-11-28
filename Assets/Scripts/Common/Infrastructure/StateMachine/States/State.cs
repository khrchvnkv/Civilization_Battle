namespace Common.Infrastructure.StateMachine.States
{
    public abstract class State : IExitableState
    {
        protected GameStateMachine StateMachine;

        public void SetupStateMachine(GameStateMachine stateMachine)
        {
            StateMachine = stateMachine;
        }
        public abstract void Exit();
    }
}