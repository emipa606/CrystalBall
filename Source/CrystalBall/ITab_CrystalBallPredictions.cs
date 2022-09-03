using CrystalBall;
using RimWorld;
using UnityEngine;
using Verse;

namespace Crystalball;

internal class ITab_CrystalBallPredictions : ITab
{
    private const float TopPadding = 20f;

    private const float ThingIconSize = 28f;

    private const float ThingRowHeight = 28f;

    private const float ThingLeftX = 36f;

    private const float StandardLineHeight = 22f;

    public static readonly Color ThingLabelColor = new Color(0.9f, 0.9f, 0.9f, 1f);

    public static readonly Color HighlightColor = new Color(0.5f, 0.5f, 0.5f, 1f);
    private Vector2 scrollPosition = Vector2.zero;

    private float scrollViewHeight;


    public ITab_CrystalBallPredictions()
    {
        size = new Vector2(460f, 450f);
        labelKey = "TabPredictions";
        tutorTag = "Predictions";
    }

    protected override void FillTab()
    {
        Text.Font = GameFont.Small;
        var rect = new Rect(0f, 20f, size.x, size.y - 20f).ContractedBy(10f);
        var position = new Rect(rect.x, rect.y, rect.width, rect.height);
        GUI.BeginGroup(position);

        Text.Font = GameFont.Small;
        GUI.color = Color.white;
        var outRect = new Rect(0f, 0f, position.width, position.height);
        var viewRect = new Rect(0f, 0f, position.width - 16f, scrollViewHeight);
        Widgets.BeginScrollView(outRect, ref scrollPosition, viewRect);

        var num = 0.0f;

        DrawKnownIncidentList(ref num, viewRect.width);

        if (Event.current.type == EventType.Layout)
        {
            scrollViewHeight = num + 30f;
        }

        Widgets.EndScrollView();
        GUI.EndGroup();
        GUI.color = Color.white;
        Text.Anchor = TextAnchor.UpperLeft;
    }

    private void DrawKnownIncidentList(ref float y, float width)
    {
        var warnedIncidentQueue = Find.World.GetComponent<WarnedIncidentQueueWorldComponent>();

        var currTick = Find.TickManager.TicksGame;

        foreach (QueuedIncident qi in warnedIncidentQueue)
        {
            var ticksLeft = qi.FireTick - currTick;

            var timeStr = ticksLeft < 60000
                ? ticksLeft.ToStringTicksToPeriodVague()
                : ticksLeft.ToStringTicksToPeriod(false, false, false);

            var incidentLabelStr = qi.FiringIncident.def.label;
            var outputString = $"Expecting {incidentLabelStr} in {timeStr}";

            Widgets.LongLabel(0.0f, width, outputString, ref y);
        }
    }
}