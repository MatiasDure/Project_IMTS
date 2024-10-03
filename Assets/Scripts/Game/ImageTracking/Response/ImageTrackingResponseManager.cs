using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using System.Linq;

public class ImageTrackingResponseManager : MonoBehaviour
{
   	[SerializeField] private ARTrackedImageManager _arTrackedImageManager;
	[SerializeField] private List<ImageObjectReference> _imageObjectReferences = new List<ImageObjectReference>();
	[SerializeField] private GameObject _imageTrackingResponsesContainer;

	private List<IImageTrackingResponse> _imageTrackingResponses = new List<IImageTrackingResponse>();
    
	private void Awake() {
		if(_arTrackedImageManager == null) throw new System.NullReferenceException("ImageTrackingSpawnResponse: ARTrackedImageManager is not set");
		if(_imageObjectReferences.Count < 1) throw new System.ArgumentException("ImageTrackingSpawnResponse: No ImageObjectReferences were added");
		if(_imageTrackingResponsesContainer == null) throw new System.NullReferenceException("ImageTrackingSpawnResponse: An empty game object container to hold the IImageTrackingResponses needs to be set for this script to work as expected");
		
		_imageTrackingResponses = _imageTrackingResponsesContainer.GetComponents<IImageTrackingResponse>().ToList();
	}

	private void OnEnable() {
		_arTrackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
	}

	private void OnDisable() {
		_arTrackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
	}

	private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs) {
		foreach (var trackedImage in eventArgs.added) {
			// Handle added tracked images
			ImageObjectReference imageObjectReference = _imageObjectReferences.FirstOrDefault(i => i.ImageName == trackedImage.referenceImage.name);
			
			// If no ImageObjectReference is found, skip to the next tracked image
			if(imageObjectReference == null)
			{
				Debug.LogError($"ImageTrackingSpawnResponse: No ImageObjectReference found for {trackedImage.referenceImage.name}");
				continue; 
			}

			foreach (var response in _imageTrackingResponses) {
				if(response.ResponseType == imageObjectReference.Response)
					response.Respond(imageObjectReference.ObjectToSpawn, trackedImage);
			} 
		}

		// foreach (var trackedImage in eventArgs.updated) {
		// 	// Handle updated tracked images
		// }

		// foreach (var trackedImage in eventArgs.removed) {
		// 	// Handle removed tracked images
		// }
	}
}
