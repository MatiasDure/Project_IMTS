using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class PassiveEventManagerEditorModeTest
{
	private PassiveEventManager passiveEventManager;

	[SetUp]
	public void Setup() {
		passiveEventManager = new GameObject().AddComponent<PassiveEventManager>();
		PassiveEventManager.Instance = passiveEventManager;
	}

    [Test]
	public void PassiveEventManager_IsEventOfCurrentPlotTest_ShouldBeTrue() {
		PlotsManager plotsManager = new GameObject().AddComponent<PlotsManager>();
		plotsManager._currentPlot = Plot.Ocean;

		Assert.IsTrue(passiveEventManager.IsEventOfCurrentPlot(plotsManager.CurrentPlot, Plot.Ocean));
	}

	[Test]
	public void PassiveEventManager_IsEventOfCurrentPlotTest_ShouldBeFalse() {
		PlotsManager plotsManager = new GameObject().AddComponent<PlotsManager>();
		plotsManager._currentPlot = Plot.Village;

		Assert.IsFalse(passiveEventManager.IsEventOfCurrentPlot(plotsManager.CurrentPlot ,Plot.Ocean));
	}

	[Test]
	public void PassiveEventManager_IsEventReadyToStart_ShouldBeTrue() {
		HideAndSea plotEvent = new GameObject().AddComponent<HideAndSea>();
		plotEvent._state = PassiveEventState.InitialReady;

		Assert.IsTrue(passiveEventManager.IsEventReadyToStart(plotEvent));
	}

	[Test]
	public void PassiveEventManager_IsEventReadyToStart_ShouldBeFalse() {
		HideAndSea plotEvent = new GameObject().AddComponent<HideAndSea>();
		plotEvent._state = PassiveEventState.Waiting;

		Assert.IsFalse(passiveEventManager.IsEventReadyToStart(plotEvent));
	}

	[Test]
	public void PassiveEventManager_HandleEventChange_ShouldChangeEventsToPassedArgs() {
		Assert.AreEqual(PassiveEvent.None, passiveEventManager.CurrentEventPlaying);
		Assert.AreEqual(PassiveEvent.None, passiveEventManager.PreviousEventPlayed);

		passiveEventManager.HandleEventChange(PassiveEvent.HideAndSea, PassiveEvent.None);

		Assert.AreEqual(PassiveEvent.HideAndSea, passiveEventManager.CurrentEventPlaying);
		Assert.AreEqual(PassiveEvent.None, passiveEventManager.PreviousEventPlayed);
	}

	[Test]
	public void PassiveEventManager_CheckEvents_ShouldStartEventIfItIsReady() {
		HideAndSea plotEvent = new GameObject().AddComponent<HideAndSea>();
		plotEvent._state = PassiveEventState.InitialReady;
		plotEvent._hideSpots = new List<Transform> { new GameObject().transform };
		PlotsManager plotsManager = new GameObject().AddComponent<PlotsManager>();
		PlotsManager.Instance = plotsManager;
		plotsManager._currentPlot = Plot.Ocean;


		PlotEventsCollection plotEventsCollection = new PlotEventsCollection();
		plotEventsCollection._plot = Plot.Ocean;
		plotEventsCollection._plotEvents = new List<PlotEvent> { plotEvent };

		passiveEventManager.CheckEvents(new List<PlotEventsCollection> { plotEventsCollection });

		Assert.AreEqual(PassiveEventState.InitialActive, plotEvent._state);
	}
}
