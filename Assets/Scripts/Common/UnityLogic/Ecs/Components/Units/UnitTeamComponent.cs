using Common.UnityLogic.Units;

namespace Common.UnityLogic.Ecs.Components.Units
{
    public struct UnitTeamComponent
    {
        public readonly UnitModel UnitModel;
        
        public bool IsActiveTeam { get; set; }
        public int AvailableRange { get; set; }
        public bool HasAvailableRange => AvailableRange > 0;

        public UnitTeamComponent(UnitModel unitModel, bool isActiveTeam)
        {
            UnitModel = unitModel;
            IsActiveTeam = isActiveTeam;
            AvailableRange = isActiveTeam ? unitModel.StaticData.Range : 0;
        }
    }
}