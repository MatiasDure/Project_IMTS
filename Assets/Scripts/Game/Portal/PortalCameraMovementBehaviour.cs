using UnityEngine;
using UnityEngine.Serialization;

public class PortalCameraMovementBehaviour : MonoBehaviour
{
	[SerializeField] internal GameObject portal;
	[SerializeField] internal GameObject worldAnchor;

	private Camera _mainCamera;
	
	private void Awake()
	{
		_mainCamera = Camera.main;
		if (_mainCamera != null) transform.rotation = _mainCamera.transform.rotation;
	}

    // Update is called once per frame
    void Update()
    {
	    if(_mainCamera == null) return;
	    
		UpdatePortalTransform(worldAnchor, portal, _mainCamera.gameObject);
    }

    internal void UpdatePortalTransform(GameObject anchor, GameObject portal, GameObject mainCameraGameObject)
    {
	    var m = anchor.transform.localToWorldMatrix * portal.transform.worldToLocalMatrix *
	            mainCameraGameObject.transform.localToWorldMatrix;
	    
	    transform.SetPositionAndRotation(m.GetColumn(3), m.rotation);
    }
}
