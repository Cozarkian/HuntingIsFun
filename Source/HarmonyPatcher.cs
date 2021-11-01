using Verse;
using HarmonyLib;

namespace HuntingIsFun
{
    [StaticConstructorOnStartup]
    public class HarmonyPatcher
    {
        static HarmonyPatcher()
        {
            Harmony harmony = new Harmony("cozarkian.huntingisfun");
            harmony.PatchAll();
        }
    }
}
