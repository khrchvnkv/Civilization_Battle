using Common.UnityLogic.Units;

namespace Common.UnityLogic.Ecs.Components.Units
{
    public struct UnitTeamComponent
    {
        public readonly TeamTypes TeamType;

        public int AvailableRange;
        public bool IsActiveTeam;

        public UnitTeamComponent(in TeamTypes teamType, in int availableRange, in bool isActiveTeam)
        {
            TeamType = teamType;
            AvailableRange = availableRange;
            IsActiveTeam = isActiveTeam;
        }
    }
}