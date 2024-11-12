using System;
using UnityEngine;

public abstract class PlotEvent : MonoBehaviour, IEvent
{
    [SerializeField] internal protected PlotEventConfig _config;
	internal protected Cooldown _cooldown = new Cooldown();
	internal protected Frequency _frequency = new Frequency();
	internal protected EventState _state;

	public EventState State { get => _state; set => _state = value; }

	public static event Action<UpdatePassiveEventCollection> OnPassiveEventStart;
	public static event Action<UpdatePassiveEventCollection> OnPasiveEventEnd;
	public event Action OnEventDone;

	public virtual void StartEvent() {
		_state = _state == EventState.InitialReady ? EventState.InitialActive : EventState.Active;
		_frequency.DecreaseFrequency();
	}

	internal protected void UpdateEventStatus()
	{
		switch(_state)
		{
			case EventState.InitialWaiting:
				_state = EventState.InitialReady;
				break;
			case EventState.InitialActive:
				CheckIfEventContinuesPlaying();
				break;
			case EventState.Waiting:
				_state = EventState.Ready;
				break;
			case EventState.Active:
				CheckIfEventContinuesPlaying();
				break;
		}
	}

	internal void CheckIfEventContinuesPlaying() {
		if(!_frequency.IsFrequencyOver()) {
			_state = EventState.Waiting;
			HandleWaitingStatus();
		} 
		else {
			_state = EventState.Done;
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
		OnEventDone?.Invoke();
	}
}