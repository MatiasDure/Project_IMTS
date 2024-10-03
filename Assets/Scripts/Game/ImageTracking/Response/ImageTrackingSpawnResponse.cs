using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ImageTrackingSpawnResponse : MonoBehaviour, IImageTrackingResponse
{
	public ImageTrackingResponses ResponseType => ImageTrackingResponses.SpawnObject;

	public void Respond(GameObject objectToSpawn, ARTrackedImage trackedImage)
	{
		Instantiate(objectToSpawn, trackedImage.transform.position, trackedImage.transform.rotation);
		objectToSpawn.transform.position = trackedImage.transform.position;
	}
}
