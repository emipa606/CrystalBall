using RimWorld;
using Verse;

namespace Crystalball;

[DefOf]
public static class ModDefs
{
    public static EffecterDef EffecterDef_Scry;
    public static JobDef JobDef_Scry;
    public static StatDef StatDef_Scry;
    public static StatDef StatDef_PredictionCount;
    public static ThingDef ThingDef_CrystalBallTable;
    public static WorkGiverDef WorkGiverDef_ScryCrystallBall;
    public static WorkTypeDef WorkTypeDef_Scry;

    static ModDefs()
    {
        DefOfHelper.EnsureInitializedInCtor(typeof(ModDefs));
    }
}