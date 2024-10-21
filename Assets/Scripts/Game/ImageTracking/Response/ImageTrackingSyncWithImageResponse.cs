using UnityEngine;

public class ImageTrackingSyncWithImageResponse : MonoBehaviour, IImageTrackingResponse
{
	public ImageTrackingResponses ResponseType => ImageTrackingResponses.SyncObjectWithImage;

	public GameObject Respond(GameObject objectToSync, GameObject trackedImage)
	{
		if(objectToSync.transform.parent != trackedImage.transform) objectToSync.transform.SetParent(trackedImage.transform);
		
		objectToSync.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
		
		return objectToSync;
	}
}
