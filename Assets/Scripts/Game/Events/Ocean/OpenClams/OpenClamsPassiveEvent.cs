using System;
using System.Collections;
using UnityEngine;

public class OpenClamsPassiveEvent : PlotEvent, IEvent, IInterruptible
{
	[SerializeField] internal ToggleObjectPassiveEvent _toggleObjectEvent;
	[SerializeField] internal ObjectMovement _beeMovement;

	internal IToggleComponent _toggleComponent;
	internal GameObject _toggleableObject;

	public event Action<IInterruptible> OnInterruptedDone;

	private void Awake() {
		_toggleObjectEvent.RetrieveToggleableObjects();
	}

    void Start()
    {
        SetUpPassiveEvent();
		SubscribeToEvents();
    }

   	private void Update() {
		_cooldown.DecreaseCooldown(Time.deltaTime);
		Debug.Log(_state);
	}

	internal void SetUpPassiveEvent() {
		_state = EventState.InitialWaiting;
		_cooldown.StartCooldown(_config.Timing.StartDelay);
		_frequency.FrequencyAmount = _config.Timing.Frequency;
	}

	private IEnumerator MoveBeeToClamp(Transform clam)
	{
		Vector3 target = clam.position + clam.forward;
		while(!_beeMovement.IsInPlace(target))
		{
			_beeMovement.MoveTo(target, 4f);
			yield return null;
		}

		_toggleComponent.Toggle();
		_toggleComponent.OnToggleDone += HandleCurrentClamToggle;
	}

	private void HandleCurrentClamToggle()
	{
		FireEndEvent(SetupEndEventMetadata());
		base.UpdateEventStatus();
		_toggleComponent.OnToggleDone -= HandleCurrentClamToggle;
	}

	public override void StartEvent()
	{
		base.StartEvent();
		UpdatePassiveEventCollection metadata = SetupStartEventMetadata(_toggleableObject.transform);

		FireStartEvent(metadata);
		StartCoroutine(MoveBeeToClamp(_toggleableObject.transform));
	}

	internal protected override void UpdateEventStatus()
	{
		if(!_toggleObjectEvent.TryGetRandomObjectWithStateOn(out IToggleComponent toggleComponent, out GameObject toggleableObject)) {
			_cooldown.StartCooldown(_state == EventState.InitialWaiting ? _config.Timing.StartDelay : _config.Timing.Cooldown);
			return;
		}
		
		_toggleComponent = toggleComponent;
		_toggleableObject = toggleableObject;
		base.UpdateEventStatus();
	}

	internal UpdatePassiveEventCollection SetupStartEventMetadata(Transform openClam)
	{
		return new UpdatePassiveEventCollection
		{
			PreviousEvent = PassiveEventManager.Instance.CurrentEventPlaying,
			CurrentEvent = PassiveEvent.OpenClams,
			State = BeeState.CloseClams,
			Metadata = new EventMetadata
			{
				Target = openClam
			}
		};
	}

	private void OnDestory() {
		UnsubscribeFromEvents();
	}

	public void InterruptEvent()
	{
		_state = _frequency.IsFrequencyOver() ? EventState.Done : EventState.Waiting;
		if(_toggleComponent != null) _toggleComponent.OnToggleDone -= HandleCurrentClamToggle;

		HandleDoneStatus();
		OnInterruptedDone?.Invoke(this);
	}
}
