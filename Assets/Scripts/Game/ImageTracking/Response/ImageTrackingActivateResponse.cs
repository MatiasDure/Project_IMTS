using UnityEngine;

public class ImageTrackingActivateResponse : MonoBehaviour, IImageTrackingResponse
{
	public ImageTrackingResponses ResponseType => ImageTrackingResponses.ActivateObject;

	public void Respond(GameObject objectToActivate, GameObject trackedImage)
	{
		objectToActivate.SetActive(true);
		objectToActivate.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 0.1f;//trackedImage.transform.position;
		objectToActivate.transform.Rotate(0, 180, 0);
	}
}