using Verse;

namespace Crystalball;

public class CrystalBallSettings : ModSettings
{
    public int CrystalBallRechargeTime = 50000; //20 hours
    public int DelayTimeFudgeWindow = 240000; //4 Days

    public string FudgeWindowEntryBuffer = "240000";

    public string MedianDelayEntryBuffer = "300000";
    public int MedianDelayTime = 300000; //5 Days
    public string RechargeTimeEntryBuffer = "50000";
    public string ScrySpeedEntryBuffer = "1.0";
    public float ScrySpeedFactor = 1.0f;

    public override void ExposeData()
    {
        Scribe_Values.Look(ref MedianDelayTime, "medianDelayTime");
        Scribe_Values.Look(ref DelayTimeFudgeWindow, "delayTimeFudgeWindow");
        Scribe_Values.Look(ref CrystalBallRechargeTime, "crystalBallRechargeTime");
        Scribe_Values.Look(ref ScrySpeedFactor, "scrySpeedFactor");

        Scribe_Values.Look(ref MedianDelayEntryBuffer, "medianDelayEntryBuffer");
        Scribe_Values.Look(ref FudgeWindowEntryBuffer, "fudgeWindowEntryBuffer");
        Scribe_Values.Look(ref RechargeTimeEntryBuffer, "rechargeTimeEntryBuffer");
        Scribe_Values.Look(ref ScrySpeedEntryBuffer, "scrySpeedEntryBuffer");

        base.ExposeData();
    }
}