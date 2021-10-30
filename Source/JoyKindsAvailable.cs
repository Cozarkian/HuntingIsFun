using RimWorld;
using Verse;
using HarmonyLib;

namespace HuntingIsFun
{
    [HarmonyPatch]
    public class JoyKindsAvailable
    {
        [HarmonyPatch(typeof(Need_Joy), "GetTipString")]
        public static void Postfix(ref string __result, Pawn ___pawn)
        {
            if (___pawn.MapHeld != null && (___pawn.story?.traits?.HasTrait(DefOf_Hunting.HIF_Hunter) ?? false))
            {
                __result = __result + "\n  - " + DefOf_Hunting.Sport.LabelCap + " (Hunting)";
            }
        }
    }
}
