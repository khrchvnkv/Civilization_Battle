using Common.Infrastructure.Services.Input;

namespace Common.Infrastructure.StateMachine.States
{
    public class GameLoopState : State, IState
    {
        private readonly IUnitsControlService _unitsControlService;

        public GameLoopState(IUnitsControlService unitsControlService) => _unitsControlService = unitsControlService;

        public void Enter() => _unitsControlService.Enable();

        public override void Exit() => _unitsControlService.Disable();
    }
}