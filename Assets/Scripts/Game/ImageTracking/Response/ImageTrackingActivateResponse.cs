using UnityEngine;

public class ImageTrackingActivateResponse : MonoBehaviour, IImageTrackingResponse
{
	public ImageTrackingResponses ResponseType => ImageTrackingResponses.ActivateObject;

	public GameObject Respond(GameObject objectToActivate, GameObject trackedImage)
	{
		objectToActivate.SetActive(true);
		objectToActivate.transform.position = trackedImage.transform.position;

		return objectToActivate;
	}
}