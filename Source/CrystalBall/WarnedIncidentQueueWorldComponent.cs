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

    private readonly Dictionary<string, int> specialIncidents = new();

    private bool isFiringEvents;
    private List<QueuedIncident> knownIncidents = [];

    private IncidentQueue warnedIncidents = new();


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
        Scribe_Deep.Look(ref warnedIncidents, "warnedIncidents");
        Scribe_Collections.Look(ref knownIncidents, "knownIncidents", LookMode.Deep);
        Scribe_Values.Look(ref warningsActivated, "warningsActivated");
    }

    private bool addKnownIncident(QueuedIncident qi)
    {
        if (knownIncidents.Find(findIncident) != null)
        {
            return false;
        }

        {
            knownIncidents.Add(qi);
            knownIncidents.Sort((a, b) => a.FireTick.CompareTo(b.FireTick));
        }

        return true;

        bool findIncident(QueuedIncident a)
        {
            return a.FireTick == qi.FireTick && a.FiringIncident.def.shortHash == qi.FiringIncident.def.shortHash;
        }
    }

    private void removeCompletedIncidentsFromKnown()
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

        var tickDelay = Rand.RangeInclusive(settings.MedianDelayTime - settings.DelayTimeFudgeWindow,
            settings.MedianDelayTime + settings.DelayTimeFudgeWindow);

        if (tickDelay < 0 || !warningsActivated)
        {
            tickDelay = 0;
        }

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
        const int retryDuration = 5000;

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
        removeCompletedIncidentsFromKnown();
        isFiringEvents = false;
    }

    private void getEventsInQueue(out List<QueuedIncident> events)
    {
        events = [];

        foreach (QueuedIncident qi in warnedIncidents)
        {
            events.Add(qi);
        }
    }

    public void PredictEvents(float predictionStrength, int maxNumPredictions)
    {
        getEventsInQueue(out var incidentList);
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

            if (addKnownIncident(qi))
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