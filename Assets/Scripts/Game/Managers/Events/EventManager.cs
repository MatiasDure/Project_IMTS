using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{
	[SerializeField] private PassiveEventManager _passiveEventManager;
	[SerializeField] private InteractionManager _interactionManager;

    private GameObject _currentEventGameObject;
	private GameObject _nextEventToPlay;
	private IEvent _currentEvent;

	private EventType _nextEventType = EventType.None;

	private void Awake()
	{
		if(_passiveEventManager == null || _interactionManager == null) throw new System.Exception("Interaction Manager or Passive Event Manager is not set in Event Manager");
	}

	private void Start()
	{
		SubscribeToEvents();
	}

	private void SubscribeToEvents()
	{
		_passiveEventManager.OnPassiveEventReadyToStart += StartInterruptionSequence;
		_interactionManager.OnInteractionReadyToStart += StartInterruptionSequence;
	}

	private void UnsubscribeFromEvents()
	{
		_passiveEventManager.OnPassiveEventReadyToStart -= StartInterruptionSequence;
		_interactionManager.OnInteractionReadyToStart -= StartInterruptionSequence;
	}

	private void StartInterruptionSequence(EventInterruption passiveEventData)
	{
		Debug.Log("Event Interruption Sequence Started");
		if(_currentEventGameObject == null)
		{
			Debug.Log($"Event Object: {passiveEventData.EventObject} ");
			UpdateCurrentEvent(passiveEventData.EventObject);
			_currentEventGameObject = passiveEventData.EventObject;
			Debug.Log($"Current Event: {_currentEventGameObject} ");
			if(passiveEventData.EventType == EventType.Passive)
				_currentEventGameObject.GetComponent<PlotEvent>().StartEvent();
			else {
				var interactable = _currentEventGameObject.GetComponent<IInteractable>();
				Debug.Log($"Interactable: {interactable} ");
				interactable.Interact();
			}
			return;
		}

		if(_currentEventGameObject == passiveEventData.EventObject) return;

		if (!_currentEventGameObject.TryGetComponent<IInterruptible>(out IInterruptible interruptibleEvent)) return;

		interruptibleEvent.InterruptEvent();
		interruptibleEvent.OnInterruptedDone += HandleEventInterrupted;
		_nextEventType = passiveEventData.EventType;
		_nextEventToPlay = passiveEventData.EventObject;
		Debug.Log("Event Is Interruptable");
	}

	private void HandleEventInterrupted(IInterruptible interruptibleEvent)
	{
		Debug.Log("Event Interrupted");
		interruptibleEvent.OnInterruptedDone -= HandleEventInterrupted;
		UpdateCurrentEvent(_nextEventToPlay);

		if (_nextEventType == EventType.Passive)
			_currentEventGameObject.GetComponent<PlotEvent>().StartEvent();
		else
		{
			var interactable = _currentEventGameObject.GetComponent<IInteractable>();
			interactable.Interact();
		}
	}

	private void UpdateCurrentEvent(GameObject nextEvent)
	{
		_currentEventGameObject = nextEvent;
		_currentEvent = _currentEventGameObject.GetComponent<IEvent>();
		_currentEvent.OnEventDone += HandleCurrentEventDone;
	}

	private void HandleCurrentEventDone() {
		_currentEventGameObject = null;
		_currentEvent.OnEventDone -= HandleCurrentEventDone;
		_currentEvent = null;
		Debug.Log("Event Done");
	}

	private void OnDestroy()
	{
		UnsubscribeFromEvents();
	}
}
