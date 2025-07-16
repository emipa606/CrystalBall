using System;
using HarmonyLib;
using RimWorld;
using Verse;

namespace CrystalBall;

[StaticConstructorOnStartup]
public static class HarmonyPatches
{
    // ReSharper disable once InconsistentNaming
    private static readonly Type patchType = typeof(HarmonyPatches);

    static HarmonyPatches()
    {
        var harmony = new Harmony("rimworld.crystallball.main");
        harmony.Patch(
            AccessTools.Method(typeof(Storyteller), nameof(Storyteller.TryFire)),
            new HarmonyMethod(patchType, nameof(TryFirePrefix)),
            new HarmonyMethod(patchType, nameof(TryFirePostfix))
        );

        harmony.Patch(
            AccessTools.Method(typeof(Storyteller), nameof(Storyteller.StorytellerTick)),
            new HarmonyMethod(patchType, nameof(StoryTellerTickPrefix)),
            new HarmonyMethod(patchType, nameof(StoryTellerTickPostfix))
        );

        //harmony.PatchAll();
    }


    public static bool TryFirePrefix(FiringIncident fi, ref bool __result, Storyteller __instance)
    {
        var warnedIncidentQueue = Find.World.GetComponent<WarnedIncidentQueueWorldComponent>();


        if (!string.IsNullOrEmpty(fi.parms?.questTag))
        {
            // Dont stop any quests
            return true;
        }

        try
        {
            if (warnedIncidentQueue.IsFiringEvents())
            {
                if (fi.def.Worker.CanFireNow(fi.parms))
                {
                    if (fi.def.Worker.TryExecute(fi.parms))
                    {
                        __result = true;
                    }
                }
                else
                {
                    __result = false;
                }

                return false;
            }

            if (fi.def.Worker.CanFireNow(fi.parms))
            {
                var incidentQueued = warnedIncidentQueue.AddIncidentToQueue(fi);
                if (!incidentQueued)
                {
                    return true;
                }

                fi.parms?.target.StoryState
                    .Notify_IncidentFired(
                        fi); //Notify the storyteller that the incident fired, so it will carry on planning the next disasters
                __result = true;
                return false;
            }

            //if we can't fire this now.... why are we trying to fire it at all...
            __result = false;
            return false;
        }
        catch
        {
            return true;
        }
    }

    public static bool TryFirePostfix(bool __result, FiringIncident fi, Storyteller __instance)
    {
        return __result;
    }

    public static bool StoryTellerTickPrefix(Storyteller __instance)
    {
        var warnedIncidentQueue = Find.World.GetComponent<WarnedIncidentQueueWorldComponent>();
        warnedIncidentQueue.WarnedIncidentQueueTick();

        return true;
    }

    public static void StoryTellerTickPostfix(Storyteller __instance)
    {
        // Do Nothing for now. Just a placeholder.
    }
}