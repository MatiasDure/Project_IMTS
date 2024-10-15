using System;
using UnityEngine;

[Serializable]
public class ImageObjectReference
{
	[SerializeField] internal string _imageName;
	[SerializeField] internal GameObject _objectReference;
	[SerializeField] internal ImageTrackingResponses _addedResponse;
	[SerializeField] internal ImageTrackingResponses _updatedResponse;

	public string ImageName => _imageName;
	public GameObject ObjectReference => _objectReference;
	public ImageTrackingResponses AddedResponse => _addedResponse;
	public ImageTrackingResponses UpdatedResponse => _updatedResponse;
}
