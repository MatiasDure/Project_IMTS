using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Update Passive events to depend on whether the user is interacting with the plot or not. 
/// Passive events should not trigger if there are active events being triggered. 
/// </summary>
public class PassiveEventManager : Singleton<PassiveEventManager>
{
	[SerializeField] private List<PlotEventsCollection> _plotEventsCollections;

	internal PassiveEvent _currentEventPlaying;
	private PassiveEvent _previousEventPlayed;

	public PassiveEvent CurrentEventPlaying => _currentEventPlaying;
	public PassiveEvent PreviousEventPlayed => _previousEventPlayed;

	public event Action<EventInterruption> OnPassiveEventReadyToStart;

	private void Start() {
		Setup();
		SubscribeToEvents();
	}

	void Update()
    {
		CheckEvents(_plotEventsCollections);
    }

	internal void CheckEvents(List<PlotEventsCollection> plotEventsCollections) {
		foreach(PlotEventsCollection plotEventsCollection in plotEventsCollections)
		{
			if (!IsEventOfCurrentPlot(PlotsManager.Instance.CurrentPlot, plotEventsCollection.Plot)) continue;
			CheckCurrentPlotEvents(plotEventsCollection.PlotEvents);
		}
	}

	private void CheckCurrentPlotEvents(List<PlotEvent> plotEvents)
	{
		foreach (PlotEvent plotEvent in plotEvents)
		{
			if (!IsEventReadyToStart(plotEvent)) continue;
			InformEventReadyToPlay(plotEvent);
		}
	}

	private void InformEventReadyToPlay(PlotEvent plotEvent) {
		EventInterruption eventInterruption = new EventInterruption(plotEvent.gameObject, EventType.Passive);
		OnPassiveEventReadyToStart?.Invoke(eventInterruption);
	}

	internal bool IsEventOfCurrentPlot(Plot currentEnvironmentPlot, Plot eventsPlot) => currentEnvironmentPlot == eventsPlot;

	internal bool IsEventReadyToStart(PlotEvent plotEvent) => plotEvent.State == EventState.InitialReady || plotEvent.State == EventState.Ready;

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

	internal void HandleEventChange(PassiveEvent newEvent, PassiveEvent previousEvent) {
		_previousEventPlayed = _currentEventPlaying;
		_currentEventPlaying = newEvent;
	}

	private void UnsubscribeFromEvents() {
		PlotEvent.OnPasiveEventEnd += HandlePassiveEventEnd;
	}

	private void OnDestroy() {
		UnsubscribeFromEvents();
	}
}
