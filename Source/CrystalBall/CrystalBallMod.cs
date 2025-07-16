using Mlie;
using UnityEngine;
using Verse;

namespace Crystalball;

public class CrystalBallMod : Mod
{
    private static string currentVersion;
    private readonly CrystalBallSettings settings;

    public CrystalBallMod(ModContentPack content) : base(content)
    {
        CrystalBallStatic.currMod = this;

        settings = GetSettings<CrystalBallSettings>();
        currentVersion =
            VersionFromManifest.GetVersionFromModMetaData(content.ModMetaData);
    }


    public override void DoSettingsWindowContents(Rect inRect)
    {
        var listingStandard = new Listing_Standard();
        listingStandard.Begin(inRect);

        listingStandard.Label("CB.Median".Translate());
        listingStandard.IntEntry(ref settings.MedianDelayTime, ref settings.MedianDelayEntryBuffer);

        listingStandard.Label("CB.DelayTime".Translate());
        listingStandard.IntEntry(ref settings.DelayTimeFudgeWindow, ref settings.FudgeWindowEntryBuffer);

        listingStandard.Label("CB.RechargeTime".Translate());
        listingStandard.IntEntry(ref settings.CrystalBallRechargeTime, ref settings.RechargeTimeEntryBuffer);

        listingStandard.Label("CB.Multiplyer".Translate());
        listingStandard.TextFieldNumeric(ref settings.ScrySpeedFactor, ref settings.ScrySpeedEntryBuffer, 0.0f,
            10.0f);
        if (currentVersion != null)
        {
            listingStandard.Gap();
            GUI.contentColor = Color.gray;
            listingStandard.Label("CB.ModVersion".Translate(currentVersion));
            GUI.contentColor = Color.white;
        }

        listingStandard.End();

        base.DoSettingsWindowContents(inRect);
    }

    public override string SettingsCategory()
    {
        return "CrystalBall";
    }
}