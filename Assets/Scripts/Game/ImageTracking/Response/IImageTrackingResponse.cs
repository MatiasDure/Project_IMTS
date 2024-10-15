using UnityEngine;

public interface IImageTrackingResponse 
{
	ImageTrackingResponses ResponseType { get; }
    public GameObject Respond(GameObject objectToSpawn, GameObject trackedImage);
}
