using Verse;

namespace Crystalball;

public class CrystalBallSettings : ModSettings
{
    public int crystalBallRechargeTime = 50000; //20 hours
    public int delayTimeFudgeWindow = 240000; //4 Days

    public bool exampleBool;
    public string fudgeWindowEntryBuffer = "240000";

    public string medianDelayEntryBuffer = "300000";
    public int medianDelayTime = 300000; //5 Days
    public string rechargeTimeEntryBuffer = "50000";
    public string scrySpeedEntryBuffer = "1.0";
    public float scrySpeedFactor = 1.0f;

    public override void ExposeData()
    {
        Scribe_Values.Look(ref medianDelayTime, "medianDelayTime");
        Scribe_Values.Look(ref delayTimeFudgeWindow, "delayTimeFudgeWindow");
        Scribe_Values.Look(ref crystalBallRechargeTime, "crystalBallRechargeTime");
        Scribe_Values.Look(ref scrySpeedFactor, "scrySpeedFactor");

        Scribe_Values.Look(ref medianDelayEntryBuffer, "medianDelayEntryBuffer");
        Scribe_Values.Look(ref fudgeWindowEntryBuffer, "fudgeWindowEntryBuffer");
        Scribe_Values.Look(ref rechargeTimeEntryBuffer, "rechargeTimeEntryBuffer");
        Scribe_Values.Look(ref scrySpeedEntryBuffer, "scrySpeedEntryBuffer");

        base.ExposeData();
    }
}