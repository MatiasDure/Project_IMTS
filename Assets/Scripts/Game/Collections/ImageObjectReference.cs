using System;
using UnityEngine;

[Serializable]
public class ImageObjectReference
{
	[SerializeField] private string _imageName;
	[SerializeField] private GameObject _objectToSpawn;
	[SerializeField] private ImageTrackingResponses _response;

	public string ImageName => _imageName;
	public GameObject ObjectToSpawn => _objectToSpawn;
	public ImageTrackingResponses Response => _response;
}
