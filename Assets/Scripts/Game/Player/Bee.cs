using System;
using UnityEngine;

public class Bee : Singleton<Bee>
{
    private BeeState _state;
	public BeeState State => _state;
	public static event Action<BeeState> OnBeeStateChanged;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
	{
		_state = BeeState.Idle;

		SubscribeToEvents();
	}

	private void SubscribeToEvents()
	{
		PlotEvent.OnPassiveEventStart += UpdateState;
		PlotEvent.OnPasiveEventEnd += UpdateState;

		InteractionEvent.OnInteractionEventStart += UpdateState;
		InteractionEvent.OnInteractionEventEnd += UpdateState;
	}

	private void UnsubscribeFromEvents()
	{
		PlotEvent.OnPassiveEventStart -= UpdateState;
		PlotEvent.OnPasiveEventEnd -= UpdateState;

		InteractionEvent.OnInteractionEventStart -= UpdateState;
		InteractionEvent.OnInteractionEventEnd -= UpdateState;
	}

	private void UpdateState(UpdatePassiveEventCollection newState) {
		_state = newState.State;
		OnBeeStateChanged?.Invoke(_state);
	}

	private void UpdateState(UpdateInteractionStateCollection newState)
    {
		_state = newState.State;
		OnBeeStateChanged?.Invoke(_state);
	}

	private void OnDestroy()
	{
		UnsubscribeFromEvents();
	}
}
