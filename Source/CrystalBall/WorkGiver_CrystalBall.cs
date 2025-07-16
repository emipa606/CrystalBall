using RimWorld;
using Verse;
using Verse.AI;

namespace Crystalball;

internal class WorkGiver_CrystalBall : WorkGiver_Scanner
{
    public override ThingRequest PotentialWorkThingRequest =>
        ThingRequest.ForDef(ModDefs.ThingDef_CrystalBallTable);

    public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
    {
        var scryAbility = pawn.GetStatValue(ModDefs.StatDef_Scry);

        if (!pawn.RaceProps.Humanlike)
        {
            return false;
        }

        if (scryAbility < 0.1)
        {
            return false;
        }

        if (!pawn.CanReserve(t, 1, -1, null, forced))
        {
            return false;
        }

        return t is Building_CrystalBallTable crystalBall && crystalBall.IsReadyForScrying();
    }

    public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
    {
        return JobMaker.MakeJob(ModDefs.JobDef_Scry, t);
    }

    public override float GetPriority(Pawn pawn, TargetInfo t)
    {
        return t.Thing.GetStatValue(ModDefs.StatDef_Scry);
    }
}