using System.Collections.Generic;
using System.Text;
using CrystalBall;
using RimWorld;
using Verse;

namespace Crystalball;

public class Building_CrystalBallTable : Building
{
    private float accumulatedPredictionCount;

    private float accumulatedScryAbility;
    private float progress;
    private bool recharged;
    private int rechargedTick;
    private float scryWorkAmount = 100.0f;

    private float
        scryWorkTickAmount =
            1.0f / 50.0f; // 1/50 = 2 hours at full speed because we need to hit a work amount of 100

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref scryWorkTickAmount, "scryWorkTickAmount", 1.0f / 50.0f);
        Scribe_Values.Look(ref scryWorkAmount, "scryWorkAmount", 100.0f);
        Scribe_Values.Look(ref progress, "progress");
        Scribe_Values.Look(ref recharged, "recharged");
        Scribe_Values.Look(ref rechargedTick, "rechargedTick");
        Scribe_Values.Look(ref accumulatedScryAbility, "accumulatedScryAbility");
        Scribe_Values.Look(ref accumulatedPredictionCount, "accumulatedPredictionCount");
    }

    public float GetCurrentProgress()
    {
        return progress / scryWorkAmount;
    }

    public void PerformScryWork(float scryingAbility, float predictionCount)
    {
        var settings = CrystalBallStatic.currMod.GetSettings<CrystalBallSettings>();

        if (!WarnedIncidentQueueWorldComponent.warningsActivated)
        {
#if DEBUG
                Log.Message("Activating the prediction queue.");
#endif
            WarnedIncidentQueueWorldComponent.warningsActivated =
                true; //One time flag, so incidents behave as normal if the crystal ball is never used.
        }

        if (recharged)
        {
            if (scryingAbility > 0.0f)
            {
                var progressTick = scryWorkTickAmount * scryingAbility * settings.scrySpeedFactor;
                progress += progressTick;

                var tickFactor = progressTick / scryWorkAmount;

                accumulatedScryAbility += scryingAbility * tickFactor;
                accumulatedPredictionCount += predictionCount * tickFactor;
            }
        }


        if (!recharged || !(progress >= scryWorkAmount))
        {
            return;
        }

        var currTime = Find.TickManager.TicksGame;
        rechargedTick = currTime + settings.crystalBallRechargeTime;
        recharged = false;
        progress = 0.0f;

#if DEBUG
                Log.Message(String.Format("Scry complete ability={0}, num={1}", scryingAbility, predictionCount));
#endif
        //perform predictions
        var warnedIncidentQueue = Find.World.GetComponent<WarnedIncidentQueueWorldComponent>();
        warnedIncidentQueue.PredictEvents(accumulatedScryAbility, (int)accumulatedPredictionCount);

        accumulatedScryAbility = 0.0f;
        accumulatedPredictionCount = 0.0f;
    }

    public bool isReadyForScrying()
    {
        return recharged;
    }

    public override void TickRare()
    {
        base.TickRare(); //Make sure any components are ticked if needed.

        //Log.Message(String.Format("Crystal Ball Recharged = {0}, CurrTick= {1}, RechargeTick={2}, Progress={3}", recharged.ToString(), Find.TickManager.TicksGame, rechargedTick, progress.ToString()));

        if (recharged)
        {
            return;
        }

        var currTime = Find.TickManager.TicksGame;
        if (currTime >= rechargedTick)
        {
            recharged = true;
        }
    }

    public override string GetInspectString()
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.Append(base.GetInspectString());

        if (!recharged)
        {
            var ticksLeft = rechargedTick - Find.TickManager.TicksGame;
            stringBuilder.AppendInNewLine(
                $"Crystal ball is recharging psychic energy: {ticksLeft.ToStringTicksToPeriod(true, true, false, false)} until complete.");
        }
        else if (progress < 1.0f)
        {
            stringBuilder.AppendInNewLine("Crystal ball is recharged and ready for use.");
        }
        else
        {
            stringBuilder.AppendInNewLine($"Progress scrying into the future: {(int)progress}%");
        }

        return stringBuilder.ToString();
    }


    public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn myPawn)
    {
        foreach (var floatMenuOption in base.GetFloatMenuOptions(myPawn))
        {
            yield return floatMenuOption;
        }

        if (!myPawn.RaceProps.Humanlike)
        {
            yield break;
        }

        var scryAbility = myPawn.GetStatValue(ModDefs.StatDef_Scry);

        if (scryAbility < 0.1f) //Make sure this number matches the work giver check
        {
            yield return new FloatMenuOption("Cannot use. Not enough intellectual and psychic sensitivity.",
                null);
        }
        else if (!recharged)
        {
            yield return new FloatMenuOption("Cannot use. Still recharging.", null);
        }
    }
}