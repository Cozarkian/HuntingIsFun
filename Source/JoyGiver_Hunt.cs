using System;
using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;
using UnityEngine;

namespace HuntingIsFun
{
    public class JoyGiver_HuntingTrip : JoyGiver
    {
		public override float GetChance(Pawn pawn)
		{
			if (!pawn.story.traits.HasTrait(DefOf_Hunting.HIF_Hunter))
			{
				return 0f;
			}
			return base.GetChance(pawn);
		}

        public override bool CanBeGivenTo(Pawn pawn)
        {
			if (MissingRequiredCapacity(pawn) != null && pawn.needs.mood?.thoughts?.memories != null)
            {
				pawn.needs.mood.thoughts.memories.TryGainMemory(DefOf_Hunting.UnableToHunt, null, null);
				return false;
            }
            return def.joyKind.PawnCanDo(pawn);
        }

        public override Job TryGiveJob(Pawn pawn)
		{
			Map map = pawn.MapHeld;
			if (map == null)
			{
				return null;
			}
			//Log.Message(pawn.LabelShort + " is planning a hunting trip.");
			bool hasThoughts = pawn.needs?.mood?.thoughts?.memories != null;
			if (!WorkGiver_HunterHunt.HasHuntingWeapon(pawn) || WorkGiver_HunterHunt.HasShieldAndRangedWeapon(pawn))
			{
				if (hasThoughts) pawn.needs.mood.thoughts.memories.TryGainMemory(DefOf_Hunting.NoHuntingWeapon, null, null);
				return null;
			}
			if (!JoyUtility.EnjoyableOutsideNow(map, null))
			{
				if (hasThoughts) pawn.needs.mood.thoughts.memories.TryGainMemory(DefOf_Hunting.BadWeatherHunt, null, null);
				return null;
			}
			Pawn target = GetTarget(pawn, map);
			if (target == null)
			{
				if (hasThoughts) pawn.needs.mood.thoughts.memories.TryGainMemory(DefOf_Hunting.NoHuntableGame, null, null);
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.Hunt, target);
			job.ignoreDesignations = true;
			//Log.Message("Sending hunting job for " + target.LabelShort);
			return job;
		}

		public static Pawn GetTarget(Pawn hunter, Map map)
        {
			List<Pawn> targets = new List<Pawn>();
			Pawn victim = null;
			int skill = (hunter.skills.GetSkill(SkillDefOf.Shooting)?.Level ?? 0) + (hunter.skills.GetSkill(SkillDefOf.Animals)?.Level ?? 0);
			bool huntsHuman = false;
			if (hunter.story.traits != null)
			{
				if (hunter.story.traits.HasTrait(TraitDefOf.Psychopath) || hunter.story.traits.HasTrait(TraitDefOf.Bloodlust) || hunter.story.traits.HasTrait(TraitDefOf.Cannibal))
				{
					huntsHuman = true;
				} 
			}
			for (int i = 0; i < map.mapPawns.AllPawnsSpawnedCount; i++)
			{
				Pawn t = map.mapPawns.AllPawnsSpawned[i];
				if (!t.Dead && t.RaceProps != null && t.Faction == null && t.ageTracker.CurLifeStage.healthScaleFactor > .5f
					&& hunter.CanReserveAndReach(t, PathEndMode.Touch, Danger.Deadly, 1, -1, null, false)
					&& (!HistoryEventUtility.IsKillingInnocentAnimal(hunter, t) || new HistoryEvent(HistoryEventDefOf.KilledInnocentAnimal, hunter.Named(HistoryEventArgsNames.Doer)).Notify_PawnAboutToDo_Job())
					&& (hunter.Ideo == null || !hunter.Ideo.IsVeneratedAnimal(t) || new HistoryEvent(HistoryEventDefOf.HuntedVeneratedAnimal, hunter.Named(HistoryEventArgsNames.Doer)).Notify_PawnAboutToDo_Job()))
				{
					if (t.RaceProps.Animal)
					{
						int value = Rand.RangeInclusive(0, (int)((t.RaceProps.baseHealthScale + t.RaceProps.wildness) * t.kindDef.combatPower));
						if (value <= skill)
						{
							targets.Add(t);
							//Log.Message(hunter.LabelShort + " is willing to hunt " + t.LabelShort + " with a value of " + value);
						}
					}
					else if (t.IsWildMan() && huntsHuman && Rand.Value < (skill/100f))
                    {
						targets.Add(t);
                    }
				}
			}
			if (targets.Count > 0)
            {
				victim = targets[Rand.RangeInclusive(0, targets.Count - 1)];
            }
			return victim;
        }
    }
}
