using System;
using System.Collections.Generic;
using log4net.Filter;
using UnityEngine;

public class HideAndSea : PlotEvent
{
	[SerializeField] private GameObject _hideSpotsContaioner;
	private List<Transform> _hideSpots;

	// public static event Action<UpdatePassiveEventCollection> OnHideStart;
	// public static event Action<UpdatePassiveEventCollection> OnHideEnd;

	private void Awake()
	{
		if (_hideSpotsContaioner == null)
			Debug.LogError("Hide spots container was not passed to HideAndSea event");		
		else
			LoadHideSpots();
	}

	private void Start() {
		SubscribeToEvents();
		SetUpPassiveEvent();
	}

	private void Update() {
		_cooldown.DecreaseCooldown(Time.deltaTime);
	}

	private void SetUpPassiveEvent() {
		_state = PassiveEventState.InitialWaiting;
		_cooldown.StartCooldown(_config.Timing.StartDelay);
		_frequency.FrequencyAmount = _config.Timing.Frequency;
	}

	private void LoadHideSpots()
	{
		_hideSpots = new List<Transform>();

		foreach (Transform child in _hideSpotsContaioner.transform)
		{
			_hideSpots.Add(child);
		}
	}

	public override void StartEvent()
	{
		base.StartEvent();

		Transform hideSpot = GetRandomHideSpot();
		UpdatePassiveEventCollection metadata = SetupStartEventMetadata(hideSpot);

		FireStartEvent(metadata);
	}

	private UpdatePassiveEventCollection SetupStartEventMetadata(Transform hideSpot)
	{
		return new UpdatePassiveEventCollection
		{
			PreviousEvent = PassiveEventManager.Instance.CurrentEventPlaying,
			CurrentEvent = PassiveEvent.HideAndSea,
			State = BeeState.Hiding,
			Metadata = new EventMetadata
			{
				Target = hideSpot
			}
		};
	}

	protected override void HandleWaitingStatus()
	{
		if(_state != PassiveEventState.Waiting) return;

		base.HandleWaitingStatus();
		UpdatePassiveEventCollection metadata = SetupEndEventMetadata();

		FireEndEvent(metadata);
	}

	protected override void SubscribeToEvents()
	{
		base.SubscribeToEvents();
		Hide.OnHidedPlayer += HandlePlayerHided;
	}

	private void HandlePlayerHided()
	{
		_cooldown.StartCooldown(_config.Timing.Duration);
	}

	private Transform GetRandomHideSpot() => _hideSpots[UnityEngine.Random.Range(0, _hideSpots.Count)];

	private void OnDestory()
	{
		UnsubscribeFromEvents();
	}
}
