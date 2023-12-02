using Common.UnityLogic.Units;

namespace Common.UnityLogic.Ecs.Components.Units
{
    public struct UnitTeamComponent
    {
        public readonly UnitModel UnitModel;
        
        public bool IsActiveTeam { get; set; }

        public UnitTeamComponent(UnitModel unitModel, bool isActiveTeam)
        {
            UnitModel = unitModel;
            IsActiveTeam = isActiveTeam;
            UnitModel.AvailableMovementRange = isActiveTeam ? unitModel.StaticData.Range : 0;
        }
    }
}