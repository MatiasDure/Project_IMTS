using UnityEngine;

public class ImageTrackingSpawnResponse : MonoBehaviour, IImageTrackingResponse
{
	public ImageTrackingResponses ResponseType => ImageTrackingResponses.SpawnObject;

	public void Respond(GameObject objectToSpawn, GameObject trackedImage)
	{
		Instantiate(objectToSpawn, trackedImage.transform.position, trackedImage.transform.rotation);
		objectToSpawn.transform.position = trackedImage.transform.position;
	}
}
