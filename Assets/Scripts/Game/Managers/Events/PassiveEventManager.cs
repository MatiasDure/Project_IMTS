using System.Collections.Generic;
using UnityEngine;

public class PassiveEventManager : Singleton<PassiveEventManager>
{
	[SerializeField] private List<PlotEventsCollection> _plotEventsCollections;

	private PassiveEvent _currentEventPlaying;
	private PassiveEvent _previousEventPlayed;

	public PassiveEvent CurrentEventPlaying => _currentEventPlaying;
	public PassiveEvent PreviousEventPlayed => _previousEventPlayed;

	protected override void Awake()
	{
		base.Awake();
	}

	private void Start() {
		Setup();
		SubscribeToEvents();
	}

	void Update()
    {
		CheckEvents(_plotEventsCollections);
    }

	private void CheckEvents(List<PlotEventsCollection> plotEventsCollections) {
		foreach(PlotEventsCollection plotEventsCollection in plotEventsCollections)
		{
			if (!IsEventOfCurrentPlot(plotEventsCollection.Plot)) continue;

			CheckCurrentPlotEvents(plotEventsCollection.PlotEvents);
		}
	}

	private void CheckCurrentPlotEvents(List<PlotEvent> plotEvents)
	{
		foreach (PlotEvent plotEvent in plotEvents)
		{
			if (!IsEventReadyToStart(plotEvent)) continue;

			plotEvent.StartEvent();
		}
	}

	internal bool IsEventOfCurrentPlot(Plot eventsPlot) => eventsPlot == PlotsManager.Instance.CurrentPlot;

	internal bool IsEventReadyToStart(PlotEvent plotEvent) => plotEvent.State == PassiveEventState.InitialReady || plotEvent.State == PassiveEventState.Ready;

	private void Setup() {
		_currentEventPlaying = PassiveEvent.None;
	}

	private void SubscribeToEvents() {
		PlotEvent.OnPassiveEventStart += HandlePassiveEventStart;
	}

	private void HandlePassiveEventStart(UpdatePassiveEventCollection eventChangeArgs)
	{
		HandleEventChange(eventChangeArgs.CurrentEvent, eventChangeArgs.PreviousEvent);
	}

	private void HandlePassiveEventEnd(UpdatePassiveEventCollection eventChangeArgs)
	{
		HandleEventChange(eventChangeArgs.CurrentEvent, eventChangeArgs.PreviousEvent);
	}

	private void HandleEventChange(PassiveEvent newEvent, PassiveEvent previousEvent) {
		_previousEventPlayed = _currentEventPlaying;
		_currentEventPlaying = newEvent;
	}

	private void UnsubscribeFromEvents() {
		PlotEvent.OnPasiveEventEnd += HandlePassiveEventEnd;
	}

	private void OnDestory() {
		UnsubscribeFromEvents();
	}
}
