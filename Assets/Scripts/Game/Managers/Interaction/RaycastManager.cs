using System;
using UnityEngine;
using UnityEngine.Serialization;

public class RaycastManager : MonoBehaviour
{
	[SerializeField] private Camera _secondaryCamera;
	[SerializeField] private GameObject _portal;
	[SerializeField] private GameObject _anchor;
	private GameObject _secondraycastPointer;
	private GameObject _mainRaycastPointer;

	private Camera _mainCamera;
	private RaycastPerspective _raycastPerspective;
	public event Action<Collider> OnRaycastHit;

    void Start()
    {
	    
	    _mainCamera = Camera.main;
	    
	    _mainRaycastPointer = _mainCamera.transform.GetChild(0).gameObject;
	    _secondraycastPointer = _secondaryCamera.transform.GetChild(0).gameObject;
	    
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
		Ray mainCameraRay = _mainCamera.ScreenPointToRay(Input.mousePosition);
		
		//Debug.DrawRay(_mainCamera.transform.position, mainCameraRay.direction * 10,Color.red,2f);
		
		if (!Physics.Raycast(mainCameraRay, out hit)) return;
		
		_mainRaycastPointer.transform.forward = mainCameraRay.direction;

		_secondraycastPointer.transform.localRotation = _mainRaycastPointer.transform.localRotation;

		Ray secondaryCameraRay = new Ray(_secondaryCamera.transform.position, _secondraycastPointer.transform.forward);
		
		//Debug.DrawRay(secondaryCameraRay.origin, secondaryCameraRay.direction * 10 ,Color.blue,2f);
		
		if (!Physics.Raycast(secondaryCameraRay, out hit)) return;

		OnRaycastHit?.Invoke(hit.collider);
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
