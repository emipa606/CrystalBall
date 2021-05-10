using UnityEngine;
using Verse;

namespace Crystalball
{
    public class CrystalBallMod : Mod
    {
        private readonly CrystalBallSettings settings;

        public CrystalBallMod(ModContentPack content) : base(content)
        {
            CrystalBallStatic.currMod = this;

            settings = GetSettings<CrystalBallSettings>();
#if DEBUG
            Log.Message("CrystalBallMode Initialized");
#endif
        }


        public override void DoSettingsWindowContents(Rect inRect)
        {
            var listingStandard = new Listing_Standard();
            listingStandard.Begin(inRect);

            listingStandard.Label("Median Delay Time");
            listingStandard.IntEntry(ref settings.medianDelayTime, ref settings.medianDelayEntryBuffer);

            listingStandard.Label("Delay Time Fudge Window");
            listingStandard.IntEntry(ref settings.delayTimeFudgeWindow, ref settings.fudgeWindowEntryBuffer);

            listingStandard.Label("Time For Crystal Balls to Recharge");
            listingStandard.IntEntry(ref settings.crystalBallRechargeTime, ref settings.rechargeTimeEntryBuffer);

            listingStandard.Label("Multipler for the speed to scry a crystal ball");
            listingStandard.TextFieldNumeric(ref settings.scrySpeedFactor, ref settings.scrySpeedEntryBuffer, 0.0f,
                10.0f);

            listingStandard.End();

            base.DoSettingsWindowContents(inRect);
        }

        public override string SettingsCategory()
        {
            return "CrystalBall";
        }
    }
}