using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class HideAndSeaTest
{
	HideAndSea hideAndSea;

	[SetUp]
	public void Setup() {
		hideAndSea = new GameObject().AddComponent<HideAndSea>();
		hideAndSea._hideSpots = new List<Transform>{
			new GameObject().transform,
			new GameObject().transform,
			new GameObject().transform
		};
		PlotEventConfig config = ScriptableObject.CreateInstance<PlotEventConfig>();
		config.Timing.StartDelay = 5f;
		config.Timing.Frequency = 2;
		hideAndSea._config = config;
		
		PassiveEventManager.Instance = new GameObject().AddComponent<PassiveEventManager>();
	}

    [Test]
	public void HideAndSea_StartEvent_ShouldSetStateToInitialActiveFromInitialReady() {
		hideAndSea._state = EventState.InitialReady;
		hideAndSea.StartEvent();

		Assert.AreEqual(EventState.InitialActive, hideAndSea._state);
	}

	[Test]
	public void HideAndSea_StartEvent_ShouldSetStateToActiveFromReady() {
		hideAndSea._state = EventState.Ready;
		hideAndSea.StartEvent();

		Assert.AreEqual(EventState.Active, hideAndSea._state);
	}

	[Test]
	public void HideAndSea_SetupStartEventMetadata_ShouldReturnCorrectHidingMetadata() {
		Transform hideSpot = new GameObject().transform;
		UpdatePassiveEventCollection metadata = hideAndSea.SetupStartEventMetadata(hideSpot);

		Assert.AreEqual(BeeState.Hiding, metadata.State);
		Assert.AreEqual(hideSpot, metadata.Metadata.Target);
		Assert.AreEqual(PassiveEvent.HideAndSea, metadata.CurrentEvent);
		Assert.AreEqual(PassiveEvent.None, metadata.PreviousEvent);
	}

	[Test]
	public void HideAndSea_LoadHideSpots_ShouldLoadTransformsFromContainer() {
		hideAndSea._hideSpotsContaioner = new GameObject();

		Transform hideSpot1 = new GameObject().transform;
		Transform hideSpot2 = new GameObject().transform;
		Transform hideSpot3 = new GameObject().transform;

		hideSpot1.SetParent(hideAndSea._hideSpotsContaioner.transform);
		hideSpot2.SetParent(hideAndSea._hideSpotsContaioner.transform);
		hideSpot3.SetParent(hideAndSea._hideSpotsContaioner.transform);

		hideAndSea.LoadHideSpots();

		Assert.AreEqual(3, hideAndSea._hideSpots.Count);
		Assert.AreEqual(hideSpot1, hideAndSea._hideSpots[0]);
		Assert.AreEqual(hideSpot2, hideAndSea._hideSpots[1]);
		Assert.AreEqual(hideSpot3, hideAndSea._hideSpots[2]);
	}

	[Test]
	public void HideAndSea_HandleWaitingStatus_ShouldFireEndEventIfStateIsWaiting() {
		hideAndSea._state = EventState.Waiting;
		
		bool isEndEventFired = false;
		PlotEvent.OnPasiveEventEnd += (metadata) => {
			isEndEventFired = true;
		};

		hideAndSea.HandleWaitingStatus();

		Assert.IsTrue(isEndEventFired); 
	} 

	[Test]
	public void HideAndSea_SetUpPassiveEvent_ShouldSetUpEventCorrectly() {
		hideAndSea.SetUpPassiveEvent();

		Assert.AreEqual(EventState.InitialWaiting, hideAndSea._state);
		Assert.IsTrue(hideAndSea._cooldown.IsOnCooldown);
		Assert.AreEqual(2, hideAndSea._frequency.FrequencyAmount);
	}

}
