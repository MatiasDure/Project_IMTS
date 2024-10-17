using UnityEngine;

public class PortalCameraMovementBehaviour : MonoBehaviour
{
	[SerializeField] private GameObject _portal;
	[SerializeField] private GameObject _worldAnchor;

	private Camera _portalCamera;
	private Camera _mainCamera;
	
	private void Awake()
	{
		_portalCamera = GetComponent<Camera>();
		_mainCamera = Camera.main;
		transform.rotation = _mainCamera.transform.rotation;
	}

    // Update is called once per frame
    void Update()
    {
	    UpdatePortalTransform(_worldAnchor, _portal, _mainCamera);
    }

    private void UpdatePortalTransform(GameObject anchor, GameObject portal, Camera mainCamera)
    {
	    var m = anchor.transform.localToWorldMatrix * portal.transform.worldToLocalMatrix *
	            mainCamera.transform.localToWorldMatrix;
	    
	    transform.SetPositionAndRotation(m.GetColumn(3), m.rotation);
    }
}
