using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ImageTrackingActivateResponse : MonoBehaviour, IImageTrackingResponse
{
	public ImageTrackingResponses ResponseType => ImageTrackingResponses.ActivateObject;

	public void Respond(GameObject objectToSpawn, ARTrackedImage trackedImage)
	{
		objectToSpawn.SetActive(true);
		objectToSpawn.transform.position = trackedImage.transform.position;
	}
}
