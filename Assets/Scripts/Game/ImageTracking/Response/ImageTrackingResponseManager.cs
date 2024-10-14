using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using System.Linq;

public class ImageTrackingResponseManager : MonoBehaviour
{
   	[SerializeField] private ARTrackedImageManager _arTrackedImageManager;
	[SerializeField] private List<ImageObjectReference> _imageObjectReferences = new List<ImageObjectReference>();
	[SerializeField] private GameObject _imageTrackingResponsesContainer;

	internal List<IImageTrackingResponse> _imageTrackingResponses = new List<IImageTrackingResponse>();
    
	private void Awake() {
		if(_arTrackedImageManager == null) Debug.LogWarning("ImageTrackingSpawnResponse: ARTrackedImageManager is not set");//throw new System.NullReferenceException("ImageTrackingSpawnResponse: ARTrackedImageManager is not set");
		
		if(_imageObjectReferences.Count < 1) Debug.LogWarning("ImageTrackingSpawnResponse: No ImageObjectReferences were added"); //throw new System.ArgumentException("ImageTrackingSpawnResponse: No ImageObjectReferences were added");
		
		if(_imageTrackingResponsesContainer == null) Debug.LogWarning("ImageTrackingSpawnResponse: An empty game object container that holds the IImageTrackingResponses needs to be set for this script to work as expected"); //throw new System.NullReferenceException("ImageTrackingSpawnResponse: An empty game object container to hold the IImageTrackingResponses needs to be set for this script to work as expected");
		else _imageTrackingResponses = _imageTrackingResponsesContainer.GetComponents<IImageTrackingResponse>().ToList();
	}

	private void OnEnable() {
		if(_arTrackedImageManager == null) return;

		_arTrackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
	}

	private void OnDisable() {
		if(_arTrackedImageManager == null) return;

		_arTrackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
	}

	private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs) {
		HandleAddedImages(eventArgs.added);
		HandleUpdatedImages(eventArgs.updated);
	}

	private void HandleAddedImages(List<ARTrackedImage> addedImages) {
		foreach (var trackedImage in addedImages)
		{
			// Handle added tracked images
			ImageObjectReference imageObjectReference = _imageObjectReferences.FirstOrDefault(i => i.ImageName == trackedImage.referenceImage.name);

			// If no ImageObjectReference is found, skip to the next tracked image
			if (imageObjectReference == null)
			{
				Debug.LogError($"ImageTrackingSpawnResponse: No ImageObjectReference found for {trackedImage.referenceImage.name}");
				continue;
			}

			HandleTrackedImageResponse(trackedImage.gameObject, imageObjectReference);
		}
	}

	private void HandleUpdatedImages(List<ARTrackedImage> addedImages)
	{
		foreach (var trackedImage in addedImages)
		{
			// Handle added tracked images
			ImageObjectReference imageObjectReference = _imageObjectReferences.FirstOrDefault(i => i.ImageName == trackedImage.referenceImage.name);

			// If no ImageObjectReference is found, skip to the next tracked image
			if (imageObjectReference == null)
			{
				Debug.LogError($"ImageTrackingSpawnResponse: No ImageObjectReference found for {trackedImage.referenceImage.name}");
				continue;
			}

			imageObjectReference.ObjectReference.transform.localPosition = Vector3.zero;
			imageObjectReference.ObjectReference.transform.localRotation = Quaternion.identity;
			//HandleTrackedImageResponse(trackedImage.gameObject, imageObjectReference);
		}
	}

	internal void HandleTrackedImageResponse(GameObject trackedImageGameObject, ImageObjectReference imageObjectReference)
	{
		foreach (var response in _imageTrackingResponses)
		{
			if (response.ResponseType == imageObjectReference.Response)
				response.Respond(imageObjectReference._objectReference, trackedImageGameObject);
		}
	}
}
