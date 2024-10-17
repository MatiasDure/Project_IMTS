using UnityEngine;

public class ImageTrackingSyncWithImageResponse : MonoBehaviour, IImageTrackingResponse
{
	public ImageTrackingResponses ResponseType => ImageTrackingResponses.SyncObjectWithImage;

	public GameObject Respond(GameObject objectToSync, GameObject trackedImage)
	{
		//if(objectToSync.transform.parent != trackedImage.transform) objectToSync.transform.SetParent(trackedImage.transform);
		
		// objectToSync.transform.SetLocalPositionAndRotatin(Vector3.zero, Quaternion.identity);

		objectToSync.transform.position = trackedImage.transform.position;
		objectToSync.transform.rotation = trackedImage.transform.rotation;
		return objectToSync;
	}
}
