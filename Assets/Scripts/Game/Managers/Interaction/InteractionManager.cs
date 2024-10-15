using UnityEngine;

[
	RequireComponent(typeof(RaycastManager)),
]
public class InteractionManager : MonoBehaviour
{
	private RaycastManager _raycastManager;

	void Awake() {
		_raycastManager = GetComponent<RaycastManager>();
	}

    void Start() {
		_raycastManager.OnRaycastHit += OnRaycastHit;	
    }

	private void OnRaycastHit(Collider collider) {
		if(!collider.TryGetComponent(out IInteractable interactable)) return;
		interactable.Interact();
	}

	void OnDestroy() {
		_raycastManager.OnRaycastHit -= OnRaycastHit;
	}
}
