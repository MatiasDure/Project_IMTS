using System.Collections.Generic;
using UnityEngine;

public class PassiveEventManager : MonoBehaviour
{
	[SerializeField] private List<PlotEventsCollection> _plotEventsCollections;

    void Update()
    {
		CheckEvents();
    }

	private void CheckEvents() {
		foreach(PlotEventsCollection plotEventsCollection in _plotEventsCollections)
		{
			if (!IsPartOfCurrentPlot(plotEventsCollection)) continue;

			foreach (PlotEvent plotEvent in plotEventsCollection.PlotEvents)
			{
				if (!IsEventReadyToStart(plotEvent)) continue;

				plotEvent.StartEvent();
			}
		}
	}

	private bool IsPartOfCurrentPlot(PlotEventsCollection plotEventsCollection) => plotEventsCollection.Plot == PlotsManager.Instance.CurrentPlot;

	private bool IsEventReadyToStart(PlotEvent plotEvent) => plotEvent.State == PassiveEventState.InitialReady || plotEvent.State == PassiveEventState.Ready;
}
