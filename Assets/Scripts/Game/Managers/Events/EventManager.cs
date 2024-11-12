using UnityEngine;

public class EventManager : MonoBehaviour
{
	[SerializeField] internal PassiveEventManager _passiveEventManager;
	[SerializeField] internal InteractionManager _interactionManager;

    private GameObject _currentEventGameObject;
	private GameObject _nextEventToPlay;
	private IEvent _currentEvent;

	private EventType _nextEventType = EventType.None;

	internal void Awake()
	{
		if(_passiveEventManager == null || _interactionManager == null) Debug.LogWarning("Interaction Manager or Passive Event Manager is not set in Event Manager");
	}

	internal void Start()
	{
		SubscribeToEvents();
	}

	private void SubscribeToEvents()
	{
		if(_passiveEventManager != null) 
			_passiveEventManager.OnPassiveEventReadyToStart += StartInterruptionSequence;
		if(_interactionManager != null) 
			_interactionManager.OnInteractionReadyToStart += StartInterruptionSequence;
	}

	private void UnsubscribeFromEvents()
	{
		if(_passiveEventManager != null) 
			_passiveEventManager.OnPassiveEventReadyToStart -= StartInterruptionSequence;
		if(_interactionManager != null) 
			_interactionManager.OnInteractionReadyToStart -= StartInterruptionSequence;
	}

	private void StartInterruptionSequence(EventInterruption passiveEventData)
	{
		if(_currentEventGameObject == null)
		{
			UpdateCurrentEvent(passiveEventData.EventObject);
			_currentEventGameObject = passiveEventData.EventObject;
			if(passiveEventData.EventType == EventType.Passive)
				_currentEventGameObject.GetComponent<PlotEvent>().StartEvent();
			else {
				var interactable = _currentEventGameObject.GetComponent<IInteractable>();
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
	}

	private void HandleEventInterrupted(IInterruptible interruptibleEvent)
	{
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
	}

	private void OnDestroy()
	{
		UnsubscribeFromEvents();
	}
}
