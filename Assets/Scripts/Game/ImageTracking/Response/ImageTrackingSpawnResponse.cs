using UnityEngine;

public class ImageTrackingSpawnResponse : MonoBehaviour, IImageTrackingResponse
{
	public ImageTrackingResponses ResponseType => ImageTrackingResponses.SpawnObject;

	public void Respond(GameObject objectToSpawn, GameObject trackedImage)
	{
		var go = Instantiate(objectToSpawn);

		go.transform.position = trackedImage.transform.position;
	}
}
