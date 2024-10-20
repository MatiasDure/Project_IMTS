using System;
using System.Collections.Generic;
using UnityEngine;

public class HideAndSea : PlotEvent
{
	[SerializeField] private GameObject _hideSpotsContaioner;
	private List<Transform> _hideSpots;

	public static event Action<UpdateBeeStateCollection> OnHideStart;
	public static event Action<UpdateBeeStateCollection> OnHideEnd;

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
		UpdateBeeStateCollection metadata = SetupStartEventMetadata(hideSpot);

		OnHideStart?.Invoke(metadata);
	}

	private UpdateBeeStateCollection SetupStartEventMetadata(Transform hideSpot)
	{
		return new UpdateBeeStateCollection
		{
			State = BeeState.Hiding,
			Metadata = new EventMetadata
			{
				Target = hideSpot
			}
		};
	}

	protected override void HandleWaitingStatus()
	{
		base.HandleWaitingStatus();
		UpdateBeeStateCollection metadata = SetupEndEventMetadata();

		OnHideEnd?.Invoke(metadata);
	}

	private Transform GetRandomHideSpot() => _hideSpots[UnityEngine.Random.Range(0, _hideSpots.Count)];

	private void OnDestory()
	{
		UnsubscribeFromEvents();
	}

}
