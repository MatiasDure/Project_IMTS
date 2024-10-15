using UnityEngine;

public class PortalCameraMovementBehaviour : MonoBehaviour
{
	[SerializeField] private GameObject _portal;
	[SerializeField] private GameObject _worldAnchor;

	private Camera _mainCamera;
	
	private void Awake()
	{
		_mainCamera = Camera.main;
	}

    // Update is called once per frame
    void Update()
    {
        MimicCameraWorldPosition();
		MimicCameraWorldRotation();
    }

	private void MimicCameraWorldPosition() {
		Vector3 userOffestFromPortal = _mainCamera.transform.position - _portal.transform.position;
		transform.position = _worldAnchor.transform.position + userOffestFromPortal;
	}

	private void MimicCameraWorldRotation() {
		float angularDifferenceBetweenPortalRotations = Quaternion.Angle(_worldAnchor.transform.rotation, _portal.transform.rotation);

		Quaternion portalRotationalDifference = Quaternion.AngleAxis(angularDifferenceBetweenPortalRotations, Vector3.up);
		Vector3 newCameraDirection = portalRotationalDifference * _mainCamera.transform.forward;
		transform.rotation = Quaternion.LookRotation(newCameraDirection, Vector3.up);
	}	
}
