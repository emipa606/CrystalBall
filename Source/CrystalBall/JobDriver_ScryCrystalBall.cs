using System.Collections.Generic;
using RimWorld;
using Verse.AI;

namespace Crystalball;

internal class JobDriver_ScryCrystalBall : JobDriver
{
    private Building_CrystalBallTable crystalBallTable => (Building_CrystalBallTable)TargetThingA;

    public override bool TryMakePreToilReservations(bool errorOnFailed)
    {
        var scryAbility = pawn.GetStatValue(ModDefs.StatDef_Scry);
        return scryAbility > 0 && pawn.Reserve(crystalBallTable, job, 1, -1, null, errorOnFailed);
    }

    protected override IEnumerable<Toil> MakeNewToils()
    {
        this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
        yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell);
        var scry = new Toil();
        scry.tickAction = delegate
        {
            var actor = scry.actor;
            var scryAbility = actor.GetStatValue(ModDefs.StatDef_Scry);
            var predictionCount = actor.GetStatValue(ModDefs.StatDef_PredictionCount);

            var crystalBall = crystalBallTable;
            crystalBall.PerformScryWork(scryAbility, predictionCount);

            actor.skills.Learn(SkillDefOf.Intellectual, 0.01f);
            actor.GainComfortFromCellIfPossible(true);
        };
        scry.FailOnCannotTouch(TargetIndex.A, PathEndMode.InteractionCell);
        scry.WithEffect(ModDefs.EffecterDef_Scry, TargetIndex.A);
        scry.WithProgressBar(TargetIndex.A, delegate
        {
            var crystalBall = crystalBallTable;
            return crystalBall?.GetCurrentProgress() ?? 0f;
        });
        scry.defaultCompleteMode = ToilCompleteMode.Delay;
        scry.defaultDuration = 500;
        scry.activeSkill = () => SkillDefOf.Intellectual;
        yield return scry;
        yield return Toils_General.Wait(2);
    }
}