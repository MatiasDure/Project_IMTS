using System;
using UnityEngine;

[Serializable]
public class ImageObjectReference
{
	[SerializeField] internal string _imageName;
	[SerializeField] internal GameObject _objectReference;
	[SerializeField] internal ImageTrackingResponses _response;

	public string ImageName => _imageName;
	public GameObject ObjectReference => _objectReference;
	public ImageTrackingResponses Response => _response;
}
