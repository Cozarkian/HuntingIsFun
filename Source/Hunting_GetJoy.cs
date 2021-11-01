using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;
using HarmonyLib;

namespace HuntingIsFun
{
    [HarmonyPatch]
    public class Hunting_GetJoy
    {
        [HarmonyPatch(typeof(JobDriver_Hunt), "MakeNewToils")]
        public static void Prefix(JobDriver_Hunt __instance)
        {
            __instance.AddFinishAction(delegate
            {
                GainHuntingJoy(__instance.pawn, __instance.Victim, (int)AccessTools.Field(typeof(JobDriver_Hunt), "jobStartTick").GetValue(__instance));
            });
        }

        public static void GainHuntingJoy(Pawn hunter, Pawn victim, int startTime)
        { 
            if (hunter.story.traits?.allTraits == null || hunter.needs?.joy == null)
            {
                return;
            }
            List<Trait> traits = hunter.story.traits.allTraits;
            for (int i = 0; i < traits.Count; i++)
            {
                TraitDef curTrait = traits[i].def;
                if (curTrait == DefOf_Hunting.Hunter)
                {
                    float time = Find.TickManager.TicksGame - startTime * .8f;
                    if (victim.Dead)
                    {
                        hunter.needs.mood.thoughts.memories.TryGainMemory(DefOf_Hunting.succHunt, null, null);
                    }
                    hunter.needs.joy.CurLevel += time;
                }
            }
        }
    }
}
