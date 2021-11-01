using RimWorld;
using Verse;

namespace HuntingIsFun
{
    [DefOf]
    public class DefOf_Hunting
    {
        public static TraitDef HIF_Hunter;
        public static ThoughtDef NoHuntingWeapon;
        public static ThoughtDef NoHuntableGame;
        public static ThoughtDef BadWeatherHunt;
        public static ThoughtDef UnableToHunt;
        public static ThoughtDef SuccessfulHunt;
        public static ThoughtDef FailedHunt;

        public static JoyKindDef Sport;

        static DefOf_Hunting()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(DefOf_Hunting));
        }
    }
}
