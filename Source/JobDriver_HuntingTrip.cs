using RimWorld;
using Verse;
using HarmonyLib;

namespace HuntingIsFun
{
    [HarmonyPatch(typeof(JobDriver_Hunt), "MakeNewToils"]
    public class JobDriver_HuntingTrip
    {
        public static void Prefix (JobDriver_Hunt __instance)
        {
            if (__instance.pawn.story?.traits != null && __instance.pawn.story.traits.HasTrait(DefOf_Hunting.Hunter))
            {
                __instance.job.ignoreDesignations = false;
            }
        }

        public static void Postfix()
        {

        }
    }
}
