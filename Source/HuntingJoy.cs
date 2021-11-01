using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;
using HarmonyLib;

namespace HuntingIsFun
{
    [HarmonyPatch]
    public static class HuntingJoy
    {
        [HarmonyPatch(typeof(JobDriver_Hunt), "MakeNewToils")]
        public static void Prefix(JobDriver_Hunt __instance)
        {
            __instance.AddFinishAction(delegate
            {
                GainHuntingJoy(__instance.job, __instance.pawn, __instance.Victim, (int)AccessTools.Field(typeof(JobDriver_Hunt), "jobStartTick").GetValue(__instance));
            });
        }

        public static void GainHuntingJoy(Job job, Pawn hunter, Pawn victim, int startTime)
        {
            //Log.Message(hunter.LabelShort + " finished hunting " + victim.LabelShort);
            if (hunter.story.traits?.allTraits == null || hunter.needs?.joy == null)
            {
                return;
            }
            List<Trait> traits = hunter.story.traits.allTraits;
            for (int i = 0; i < traits.Count; i++)
            {
                TraitDef curTrait = traits[i].def;
                if (curTrait == DefOf_Hunting.HIF_Hunter)
                {
                    float time = Find.TickManager.TicksGame - startTime; 
                    //Log.Message("The trip lasted " + time + " ticks.");
                    hunter.needs.joy.GainJoy(time * .36f / 4000, DefOf_Hunting.Sport);
                    if (hunter.needs?.mood?.thoughts?.memories == null)
                    {
                        return;
                    }
                    if (victim.Dead)
                    {
                        hunter.needs.mood.thoughts.memories.TryGainMemory(DefOf_Hunting.SuccessfulHunt, null, null);
                    }                
                    else hunter.needs.mood.thoughts.memories.TryGainMemory(DefOf_Hunting.FailedHunt, null, null);
                }
            }
        }
    }
}
