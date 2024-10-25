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
		
		IInteractable[] _interactables = collider.GetComponents<IInteractable>();
		
		if(_interactables == null || _interactables.Length<=0) return;
		
		foreach (var interactable in _interactables)
		{
			interactable.Interact();
		}
		
	}

	void OnDestroy() {
		_raycastManager.OnRaycastHit -= OnRaycastHit;
	}
}
