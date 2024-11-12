using System;
using UnityEngine;

[
	RequireComponent(typeof(RaycastManager)),
]
public class InteractionManager : MonoBehaviour
{
	private RaycastManager _raycastManager;

	public event Action<EventInterruption> OnInteractionReadyToStart;
	
	void Awake() {
		_raycastManager = GetComponent<RaycastManager>();
	}

    void Start() {
		_raycastManager.OnRaycastHit += OnRaycastHit;	
    }

	private void OnRaycastHit(Collider collider) {
		
		IInteractable interactable = collider.GetComponent<IInteractable>();
		Debug.Log("here in interaction manager");
		
		if(interactable == null) return;
		Debug.Log("here in interaction manager2");
		
		if(interactable.CanInterrupt) {
			EventInterruption eventInterruption = new EventInterruption(collider.gameObject, EventType.Active);
			OnInteractionReadyToStart?.Invoke(eventInterruption);
			return;
		} 
		
		interactable.Interact();
	}

	void OnDestroy() {
		_raycastManager.OnRaycastHit -= OnRaycastHit;
	}
}
