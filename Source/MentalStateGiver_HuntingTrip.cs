using RimWorld;
using Verse;

namespace HuntingIsFun
{
    public class MentalStateGiver_HuntingTrip : TraitMentalStateGiver
    {
        public override bool CheckGive(Pawn pawn, int checkInterval)
        {
            int joyCat = (int)pawn.needs.joy.CurCategory;
            float mtb = traitDegreeData.randomMentalStateMtbDaysMoodCurve.Evaluate(joyCat);
            bool forageTime = Rand.MTBEventOccurs(mtb, 60000f, (float)checkInterval);
            //if (forageTime) Log.Message(pawn.Name + " is foraging with a score of " + mtb.ToString());
            return forageTime && this.traitDegreeData.randomMentalState.Worker.StateCanOccur(pawn) && pawn.mindState.mentalStateHandler.TryStartMentalState(this.traitDegreeData.randomMentalState, "MentalStateReason_Trait".Translate(this.traitDegreeData.GetLabelFor(pawn)), false, false, null, false);
        }
    }
}
