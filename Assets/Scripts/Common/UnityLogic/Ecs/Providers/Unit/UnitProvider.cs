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

            var unitTeamComponent =
                new UnitTeamComponent(unit.TeamType, unit.StaticData.Range, unit.TeamType == DefaultTeam);
            AddComponent(unitTeamComponent);
        }
    }
}