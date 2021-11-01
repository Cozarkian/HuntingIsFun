using RimWorld;
using Verse;
using Verse.AI;

namespace HuntingIsFun
{
    class MentalStateWorker_HuntingTrip : MentalStateWorker
    {
        public override bool StateCanOccur(Pawn pawn)
        {
            MemoryThoughtHandler curMemories = pawn.needs.mood.thoughts.memories;
            if (pawn.Drafted || HealthAIUtility.ShouldBeTendedNowByPlayer(pawn) ||
            {
                return false;
            }
            return base.StateCanOccur(pawn);
        }
    }
    {
    }
}
