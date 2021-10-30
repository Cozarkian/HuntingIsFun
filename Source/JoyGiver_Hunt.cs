using System;
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
			Pawn victim = null;
			int shooting = hunter.skills.GetSkill(SkillDefOf.Shooting)?.Level ?? 1;
			int animals = hunter.skills.GetSkill(SkillDefOf.Animals)?.Level ?? 1;
			int average = Mathf.RoundToInt((shooting + animals) / 2);
			int num = Math.Max(shooting, animals) * average + 1; //Log.Message("Starting number is " + num);
			for (int i = 0; i < map.mapPawns.AllPawnsSpawnedCount; i++)
			{
				Pawn t = map.mapPawns.AllPawnsSpawned[i];
				if (!t.Dead && t.RaceProps != null && t.RaceProps.Animal && t.Faction == null
					&& hunter.CanReserveAndReach(t, PathEndMode.Touch, Danger.Deadly, 1, -1, null, false)
					&& (!HistoryEventUtility.IsKillingInnocentAnimal(hunter, t) || new HistoryEvent(HistoryEventDefOf.KilledInnocentAnimal, hunter.Named(HistoryEventArgsNames.Doer)).Notify_PawnAboutToDo_Job())
					&& (hunter.Ideo == null || !hunter.Ideo.IsVeneratedAnimal(t) || new HistoryEvent(HistoryEventDefOf.HuntedVeneratedAnimal, hunter.Named(HistoryEventArgsNames.Doer)).Notify_PawnAboutToDo_Job()))
				{
					int value= Rand.RangeInclusive(0,(int)(t.RaceProps.wildness * t.kindDef.combatPower));
					if (value >= average && value < num)
                    {
						//Log.Message(t.LabelShort + " has value of " + value);
						num = value;
						victim = t;
                    }
				} 
			}
			return victim;
        }
    }
}
