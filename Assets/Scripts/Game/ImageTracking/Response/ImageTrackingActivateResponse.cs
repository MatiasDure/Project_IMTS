using UnityEngine;

public class ImageTrackingActivateResponse : MonoBehaviour, IImageTrackingResponse
{
	public ImageTrackingResponses ResponseType => ImageTrackingResponses.ActivateObject;

	public void Respond(GameObject objectToActivate, GameObject trackedImage)
	{
		objectToActivate.SetActive(true);
		objectToActivate.transform.SetParent(trackedImage.transform);
		objectToActivate.transform.localPosition = Vector3.zero;
	}
}