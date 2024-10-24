using System;
using UnityEngine;

public abstract class PlotEvent : MonoBehaviour
{
    [SerializeField] internal protected PlotEventConfig _config;
	internal protected Cooldown _cooldown = new Cooldown();
	internal protected Frequency _frequency = new Frequency();
	internal protected PassiveEventState _state;

	public PassiveEventState State => _state;

	public static event Action<UpdatePassiveEventCollection> OnPassiveEventStart;
	public static event Action<UpdatePassiveEventCollection> OnPasiveEventEnd;
	
	public virtual void StartEvent() {
		_state = _state == PassiveEventState.InitialReady ? PassiveEventState.InitialActive : PassiveEventState.Active;
		_frequency.DecreaseFrequency();
	}

	internal protected void UpdateEventStatus()
	{
		switch(_state)
		{
			case PassiveEventState.InitialWaiting:
				_state = PassiveEventState.InitialReady;
				break;
			case PassiveEventState.InitialActive:
				CheckIfEventContinuesPlaying();
				break;
			case PassiveEventState.Waiting:
				_state = PassiveEventState.Ready;
				break;
			case PassiveEventState.Active:
				CheckIfEventContinuesPlaying();
				break;
		}
	}

	internal void CheckIfEventContinuesPlaying() {
		if(!_frequency.IsFrequencyOver()) {
			_state = PassiveEventState.Waiting;
			HandleWaitingStatus();
		} 
		else {
			_state = PassiveEventState.Done;
			HandleDoneStatus();
		}
	}

	internal protected virtual void HandleWaitingStatus() {		
		_cooldown.StartCooldown(_config.Timing.Cooldown);
	}

	internal protected virtual void HandleDoneStatus() {
		UpdatePassiveEventCollection eventMetadata = SetupEndEventMetadata();

		FireEndEvent(eventMetadata);
	}

	protected virtual void SubscribeToEvents() {
		_cooldown.OnCooldownOver += UpdateEventStatus;
	}

	protected virtual void UnsubscribeFromEvents() {
		_cooldown.OnCooldownOver -= UpdateEventStatus;
	}

	protected virtual UpdatePassiveEventCollection SetupEndEventMetadata() => 
		new UpdatePassiveEventCollection{
			PreviousEvent = PassiveEventManager.Instance.CurrentEventPlaying,
		};

	internal protected void FireStartEvent(UpdatePassiveEventCollection eventMetadata) {
		OnPassiveEventStart?.Invoke(eventMetadata);
	}

	internal protected void FireEndEvent(UpdatePassiveEventCollection eventMetadata) {
		OnPasiveEventEnd?.Invoke(eventMetadata);
	}
}