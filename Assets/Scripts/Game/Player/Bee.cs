using System;
using UnityEngine;

public class Bee : Singleton<Bee>
{
    private BeeState _state;

	public BeeState State => _state;

	public static event Action<BeeState> OnBeeStateChanged;

	private void Start()
	{
		_state = BeeState.FollowingCamera;

		SubscribeToEvents();
	}

	private void SubscribeToEvents()
	{
		PlotEvent.OnPassiveEventStart += UpdateState;
		PlotEvent.OnPasiveEventEnd += UpdateState;
	}

	private void UnsubscribeFromEvents()
	{
		PlotEvent.OnPassiveEventStart -= UpdateState;
		PlotEvent.OnPasiveEventEnd -= UpdateState;
	}

	private void UpdateState(UpdatePassiveEventCollection newState) {
		_state = newState.State;
		OnBeeStateChanged?.Invoke(_state);
	}

	public void UpdateState(BeeState newState) {
		_state = newState;
		OnBeeStateChanged?.Invoke(_state);
	}

	private void OnDestroy()
	{
		UnsubscribeFromEvents();
	}
}
