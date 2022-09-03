using System;
using System.Collections;
using System.Collections.Generic;
using Crystalball;
using RimWorld;
using RimWorld.Planet;
using Verse;

namespace CrystalBall;

public class WarnedIncidentQueueWorldComponent : WorldComponent
{
    private const float predictionDecayFactor = 0.8f;

    public static bool warningsActivated;

    private readonly Dictionary<string, int> specialIncidents = new Dictionary<string, int>();

    private bool isFiringEvents;
    private List<QueuedIncident> knownIncidents = new List<QueuedIncident>();

    private IncidentQueue warnedIncidents = new IncidentQueue();


    public WarnedIncidentQueueWorldComponent(World world) : base(world)
    {
        specialIncidents.Add("IncidentWorker_CaravanDemand", 0);
        specialIncidents.Add("IncidentWorker_CaravanMeeting", 0);
        specialIncidents.Add("IncidentWorker_CaravanArrivalTributeCollector", 0);
        specialIncidents.Add("IncidentWorker_GiveQuest", 0);
        specialIncidents.Add("IncidentWorker_Ambush", 0);
        specialIncidents.Add("IncidentWorker_Ambush_EnemyFaction", 0);
        specialIncidents.Add("IncidentWorker_Ambush_ManhunterPack", 0);
    }

    public override void ExposeData()
    {
        Scribe_Deep.Look(ref warnedIncidents, "warnedIncidents", Array.Empty<object>());
        Scribe_Collections.Look(ref knownIncidents, "knownIncidents", LookMode.Deep, Array.Empty<object>());
        Scribe_Values.Look(ref warningsActivated, "warningsActivated");
    }

    private bool AddKnownIncident(QueuedIncident qi)
    {
        bool FindIncident(QueuedIncident a)
        {
            return a.FireTick == qi.FireTick && a.FiringIncident.def.shortHash == qi.FiringIncident.def.shortHash;
        }

        if (knownIncidents.Find(FindIncident) != null)
        {
            return false;
        }

        {
            knownIncidents.Add(qi);
            knownIncidents.Sort((a, b) => a.FireTick.CompareTo(b.FireTick));
        }

        return true;
    }

    private void RemoveCompletedIncidentsFromKnown()
    {
        var startSize = knownIncidents.Count;

        var currTick = Find.TickManager.TicksGame;
        knownIncidents.RemoveAll(a => a.FireTick < currTick);

        if (startSize > knownIncidents.Count)
        {
            knownIncidents.Sort((a, b) => a.FireTick.CompareTo(b.FireTick));
        }
    }

    public bool AddIncidentToQueue(FiringIncident fi)
    {
        var settings = CrystalBallStatic.currMod.GetSettings<CrystalBallSettings>();

        var tickDelay = Rand.RangeInclusive(settings.medianDelayTime - settings.delayTimeFudgeWindow,
            settings.medianDelayTime + settings.delayTimeFudgeWindow);

        if (tickDelay < 0 || !warningsActivated)
        {
            tickDelay = 0;
        }

#if DEBUG
            Log.Message(String.Format("Adding incident TickDelay={0}", tickDelay));
#endif

        if (specialIncidents.ContainsKey(fi.def.workerClass.ToString()))
        {
            specialIncidents.TryGetValue(fi.def.workerClass.ToString(), tickDelay);
        }

        if (tickDelay <= 0)
        {
            return false;
        }

        var currTick = Find.TickManager.TicksGame;
        var fireTick = currTick + tickDelay;
        var retryDuration = 5000;

        warnedIncidents.Add(fi.def, fireTick, fi.parms, retryDuration);

        return true;
    }

    public bool IsFiringEvents()
    {
        return isFiringEvents;
    }

    public void WarnedIncidentQueueTick()
    {
        isFiringEvents = true;
        warnedIncidents.IncidentQueueTick();
        RemoveCompletedIncidentsFromKnown();
        isFiringEvents = false;
    }

    private void GetEventsInQueue(out List<QueuedIncident> events)
    {
        events = new List<QueuedIncident>();

        foreach (QueuedIncident qi in warnedIncidents)
        {
            events.Add(qi);
        }
    }

    public void PredictEvents(float predictionStrength, int maxNumPredictions)
    {
#if DEBUG
            Log.Message(String.Format("Predicting with strength={0}, num={1}", predictionStrength, maxNumPredictions));
#endif


        GetEventsInQueue(out var incidentList);
        incidentList.Shuffle();

        var count = 0;
        foreach (var qi in incidentList)
        {
            var predictionSuccess = Rand.Chance(predictionStrength);

            if (!predictionSuccess)
            {
                continue;
            }

            Log.Message($"Prediction Success strength={predictionStrength}");

            if (AddKnownIncident(qi))
            {
                Log.Message($"Added Incident {qi.FiringIncident.def.defName}");

                predictionStrength *= predictionDecayFactor;
                count++;
            }

            if (count >= maxNumPredictions)
            {
                return;
            }
        }
    }

    public IEnumerator GetEnumerator()
    {
        foreach (var qi in knownIncidents)
        {
            yield return qi;
        }
    }
}