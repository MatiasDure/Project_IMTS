using System.Collections.Generic;
using UnityEngine;

public class HideAndSea : PlotEvent
{
	[SerializeField] internal GameObject _hideSpotsContaioner;
	internal List<Transform> _hideSpots;

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

	internal void SetUpPassiveEvent() {
		_state = PassiveEventState.InitialWaiting;
		_cooldown.StartCooldown(_config.Timing.StartDelay);
		_frequency.FrequencyAmount = _config.Timing.Frequency;
	}

	internal void LoadHideSpots()
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
		Transform hideSpot = GetRandomHideSpot(_hideSpots, UnityEngine.Random.Range(0, _hideSpots.Count));
		UpdatePassiveEventCollection metadata = SetupStartEventMetadata(hideSpot);

		FireStartEvent(metadata);
	}

	internal UpdatePassiveEventCollection SetupStartEventMetadata(Transform hideSpot)
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

	internal protected override void HandleWaitingStatus()
	{
		if(_state != PassiveEventState.Waiting) return;

		base.HandleWaitingStatus();
		UpdatePassiveEventCollection metadata = SetupEndEventMetadata();

		FireEndEvent(metadata);
	}

	protected override void SubscribeToEvents()
	{
		base.SubscribeToEvents();
		Hide.OnHidedPlayer += HandlePlayerHidden;
	}

	private void HandlePlayerHidden()
	{
		_cooldown.StartCooldown(_config.Timing.Duration);
	}

	protected override void UnsubscribeFromEvents()
	{
		base.UnsubscribeFromEvents();
		Hide.OnHidedPlayer -= HandlePlayerHidden;
	}

	internal Transform GetRandomHideSpot(List<Transform> hideSpots, int randomIndex) => hideSpots[randomIndex];

	private void OnDestory()
	{
		UnsubscribeFromEvents();
	}
}
