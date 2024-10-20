using UnityEngine;

public abstract class PlotEvent : MonoBehaviour
{
    [SerializeField] protected PlotEventConfig _config;
	protected Cooldown _cooldown = new Cooldown();
	protected PassiveEventState _state;

	public PassiveEventState State => _state;
	
	public virtual void StartEvent() {
		_state = _state == PassiveEventState.InitialReady ? PassiveEventState.InitialActive : PassiveEventState.Active;

		_cooldown.StartCooldown(_config.Timing.Duration);
	}

	protected void HandleCooldownFinished()
	{
		switch(_state)
		{
			case PassiveEventState.InitialWaiting:
				_state = PassiveEventState.InitialReady;
				break;
			case PassiveEventState.InitialActive:
				_state = PassiveEventState.Waiting;
				HandleWaitingStatus();
				break;
			case PassiveEventState.Waiting:
				_state = PassiveEventState.Ready;
				break;
			case PassiveEventState.Active:
				if(_config.Timing.Frequency > 0) {
					_state = PassiveEventState.Waiting;
					HandleWaitingStatus();
				} 
				else 
					_state = PassiveEventState.Done;
				break;
		}
	}

	protected virtual void HandleWaitingStatus() {
		if(_state != PassiveEventState.Waiting) return;
		
		_cooldown.StartCooldown(_config.Timing.Cooldown);
	}

	protected void SubscribeToEvents() {
		_cooldown.OnCooldownOver += HandleCooldownFinished;
	}

	protected void UnsubscribeFromEvents() {
		_cooldown.OnCooldownOver -= HandleCooldownFinished;
	}

	protected UpdateBeeStateCollection SetupEndEventMetadata() => new UpdateBeeStateCollection();
}