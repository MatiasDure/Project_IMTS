using UnityEngine;

public interface IImageTrackingResponse 
{
	ImageTrackingResponses ResponseType { get; }
    public void Respond(GameObject objectToSpawn, GameObject trackedImage);
}
