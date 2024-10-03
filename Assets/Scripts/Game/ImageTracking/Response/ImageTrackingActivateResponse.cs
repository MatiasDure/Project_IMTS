using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ImageTrackingActivateResponse : MonoBehaviour, IImageTrackingResponse
{
	public ImageTrackingResponses ResponseType => ImageTrackingResponses.ActivateObject;

	public void Respond(GameObject objectToActivate, GameObject trackedImage)
	{
		objectToActivate.SetActive(true);
		objectToActivate.transform.position = trackedImage.transform.position;
	}
}