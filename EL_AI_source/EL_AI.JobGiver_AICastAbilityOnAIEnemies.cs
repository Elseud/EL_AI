using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI;
using RimWorld;

namespace EL_AI
{
    public class JobGiver_AICastAbilityOnAIEnemies : JobGiver_AICastAbility
    {
        Random rand = new Random();

        public JobGiver_AICastAbilityOnAIEnemies() { }

        static JobGiver_AICastAbilityOnAIEnemies() { }

        protected override Job TryGiveJob(Pawn pawn)
        {
            if (pawn.CurJobDef == this.ability.jobDef)
            {
                return (Job)null;
            }

            Ability ability = pawn.abilities?.GetAbility(this.ability);
            if (ability == null || !ability.CanCast)
            {
                return (Job)null;
            }

            LocalTargetInfo target = this.GetTarget(pawn, ability);
            if (target.Thing.Faction == null)
            {
                return (Job)null;
            }

            if (target.Thing.Faction.AllyOrNeutralTo(pawn.Faction) == true)
            {
                return (Job)null;
            }

            if (!target.IsValid)
            {
                return (Job)null;
            }

            return ability.GetJob(target, target);
        }
        protected override LocalTargetInfo GetTarget(Pawn caster, Ability ability)
        {
            float range = ability.def.verbProperties.range;
            List<Pawn> enemyPawns = caster.Map.mapPawns.AllPawns.Where((Pawn x) => x.Position.DistanceTo(caster.Position) <= range && caster.Faction.HostileTo(x.Faction)).ToList();
            return new LocalTargetInfo(enemyPawns[rand.Next(enemyPawns.Count())]);
        }
    }
}
