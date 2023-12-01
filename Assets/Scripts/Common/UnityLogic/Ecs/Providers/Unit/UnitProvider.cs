using System;
using Common.UnityLogic.Ecs.Components.Units;
using Common.UnityLogic.Units;

namespace Common.UnityLogic.Ecs.Providers.Unit
{
    public sealed class UnitProvider : MonoProvider
    {
        private const TeamTypes DefaultTeam = TeamTypes.TeamA;
        
        protected override void EnableEntity()
        {
            base.EnableEntity();

            var unit = gameObject.GetComponent<Units.Unit>();
            if (unit is null) throw new Exception("Unit provider must have Unit component");

            var unitModel = unit.Model;
            var unitTeamComponent =
                new UnitTeamComponent(unitModel.TeamType, unitModel.StaticData.Range, unitModel.TeamType == DefaultTeam);
            AddComponent(unitTeamComponent);
        }
    }
}