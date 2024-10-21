using System;
using UnityEngine;
using UnityEngine.Serialization;

public class RaycastManager : MonoBehaviour
{
	[SerializeField] internal GameObject _secondaryCamera;
	private GameObject _secondraycastPointer;
	private GameObject _mainRaycastPointer;

	private Camera _mainCamera;
	private RaycastPerspective _raycastPerspective;
	public event Action<Collider> OnRaycastHit;

    void Start()
    {
	    _mainCamera = Camera.main;
	    
	    if(_mainCamera != null)
			_mainRaycastPointer = _mainCamera.transform.GetChild(0).gameObject;
	    if(_secondaryCamera != null)
			_secondraycastPointer = _secondaryCamera.transform.GetChild(0).gameObject;
	    
        _raycastPerspective = RaycastPerspective.SecondaryCamera;
    }

    // Update is called once per frame
    void Update()
    {
	    if(_mainCamera == null || InputManager.Instance.InputState != InputState.Interact) return;

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
		Ray mainCameraRay = _mainCamera.ScreenPointToRay(Input.mousePosition);
		
		if (!Physics.Raycast(mainCameraRay, out hit)) return;
		
		Ray secondaryCameraRay = SecondCameraRay(mainCameraRay,
					_mainRaycastPointer,_secondraycastPointer);
		
		if (!Physics.Raycast(secondaryCameraRay, out hit)) return;

		OnRaycastHit?.Invoke(hit.collider);
	}

	internal Ray SecondCameraRay(Ray mainCameraRay, GameObject mainRaycastPointer, GameObject secondRaycastPointer)
	{
		mainRaycastPointer.transform.forward = mainCameraRay.direction;
		secondRaycastPointer.transform.localRotation = mainRaycastPointer.transform.localRotation;

		Ray secondaryCameraRay = new Ray(_secondaryCamera.transform.position, secondRaycastPointer.transform.forward);
		
		return secondaryCameraRay;
	}
	
	private void MainCameraRaycast() {
		WorldRaycast();
	}

	private void WorldRaycast() {
		RaycastHit hit;
		Ray mainCameraRay = _mainCamera.ScreenPointToRay(Input.mousePosition);
		
		if (!Physics.Raycast(mainCameraRay, out hit)) return;
		
		OnRaycastHit?.Invoke(hit.collider);
	}
}
