using System;
using UnityEngine;

public class RaycastManager : MonoBehaviour
{
	[SerializeField] private Camera _secondaryCamera;

	private RaycastPerspective _raycastPerspective;
	public event Action<Collider> OnRaycastHit;

    void Start()
    {
        _raycastPerspective = RaycastPerspective.SecondaryCamera;
    }

    // Update is called once per frame
    void Update()
    {
        if(InputManager.Instance.InputState != InputState.Interact) return;

		Raycast();
    }

	private void Raycast() {
		switch(_raycastPerspective) {
			case RaycastPerspective.MainCamera:
				MainCameraRaycast();
				break;
			case RaycastPerspective.SecondaryCamera:
				SecondaryCameraRaycast();
				break;
			default:
				MainCameraRaycast();
				break;
		}
	}

	private void SecondaryCameraRaycast()
	{
		RaycastHit hit;
		Ray mainCameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);

		if (!Physics.Raycast(mainCameraRay, out hit)) return;
		
		Ray secondaryCameraRay = new Ray(_secondaryCamera.transform.position, mainCameraRay.direction);

		if (!Physics.Raycast(secondaryCameraRay, out hit)) return;

		OnRaycastHit?.Invoke(hit.collider);
	}

	private void MainCameraRaycast() {
		WorldRaycast();
	}

	private void WorldRaycast() {
	}
}
