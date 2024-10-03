using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public interface IImageTrackingResponse 
{
	ImageTrackingResponses ResponseType { get; }
    public void Respond(GameObject objectToSpawn, GameObject trackedImage);
}
