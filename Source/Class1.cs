using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using RimWorld.Planet;
using Verse;
using Verse.AI;
using Unity;

namespace HuntingIsFun
{
    public class MentalState_BugForaging : MentalState
    {
        public override RandomSocialMode SocialModeMax()
        {
            return RandomSocialMode.Quiet;
        }

        public override void MentalStateTick()
        {
            if (pawn.IsHashIntervalTick(30))
            {
                base.MentalStateTick();
                if (HealthAIUtility.ShouldBeTendedNowByPlayer(pawn))
                {
                    //Log.Message("Not a good time for foraging");
                    RecoverFromState();
                    return;
                }
            }
        }

        public override void PostEnd()
        {
            pawn.needs.mood.thoughts.memories.TryGainMemory(def.moodRecoveryThought, null);
            List<Pawn> otherPawns = new List<Pawn>();
            Pawn otherPawn;
            if (pawn.Map != null)
            {
                otherPawns = pawn.Map.mapPawns.AllPawnsSpawned;
            }
            else if (pawn.IsCaravanMember())
            {
                otherPawns = pawn.GetCaravan().PawnsListForReading;
            }
            for (int i = 0; i < otherPawns.Count; i++)
            {
                otherPawn = otherPawns[i];
                if (!otherPawn.NonHumanlikeOrWildMan() && otherPawn.needs != null)
                {
                    otherPawn.needs.mood.thoughts.memories.TryGainMemory(Entomophagy_DefOf.ObservedForaging, pawn);
                }
            }
        }
    }
}
