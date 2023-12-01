using System;
using Common.UnityLogic.Ecs.Components.Units;
using Common.UnityLogic.Ecs.Systems.Battle;

namespace Common.UnityLogic.Ecs.Providers.Unit
{
    public sealed class UnitProvider : MonoProvider
    {
        protected override void EnableEntity()
        {
            base.EnableEntity();

            var unit = gameObject.GetComponent<Units.Unit>();
            if (unit is null) throw new Exception("Unit provider must have Unit component");

            var unitModel = unit.Model;
            var isActiveTeam = unitModel.TeamType == BattleSystem.DefaultTeam;
            var unitTeamComponent = new UnitTeamComponent(unitModel, isActiveTeam);
            AddComponent(unitTeamComponent);
        }
    }
}