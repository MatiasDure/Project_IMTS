using System;
using System.Collections;
using System.Collections.Generic;
using Codice.Foreign;
using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class AnchorManager : MonoBehaviour
{
	private const string ANCHOR_GAMEOBJECT_NAME = "AnchorObject";

	[SerializeField] private ARAnchorManager _anchorManager;
	[SerializeField] private ImageTrackingResponseManager _arTrackedImageManager;

	private List<ImageAnchorCollection> _imageAnchorCollections = new List<ImageAnchorCollection>();

	public event Action<ImageAnchorCollection> OnAnchorTracked;
    
	// Start is called before the first frame update
    void Start()
	{
		if (_anchorManager == null) Debug.LogWarning("AnchorManager: ARAnchorManager is not set");
		if (_arTrackedImageManager == null) Debug.LogWarning("AnchorManager: ARTrackedImageManager is not set");
		
		SubscribeToEvents();
	}

	private void HandleAnchorsChanged(ARAnchorsChangedEventArgs args)
	{
		foreach (var anchor in args.added)
		{
			ImageAnchorCollection? imageAnchorCollection = GetImageAnchorCollection(anchor.gameObject);

			if (imageAnchorCollection == null) continue;

			OnAnchorTracked?.Invoke(imageAnchorCollection.Value);
		}
	}

	private void HandleAddedImages(ImageAnchorCollection imageAnchorCollection) {
			GameObject anchorObject = new GameObject(ANCHOR_GAMEOBJECT_NAME);
			anchorObject.transform.SetPositionAndRotation(imageAnchorCollection.Image.transform.position, imageAnchorCollection.Image.transform.rotation);
			anchorObject.AddComponent<ARAnchor>();

			TrackImageAnchor(imageAnchorCollection, anchorObject);
	}

	private void TrackImageAnchor(ImageAnchorCollection imageCollection, GameObject anchorObject) {
		_imageAnchorCollections.Add(new ImageAnchorCollection { Image = imageCollection.Image, PlotObject = imageCollection.PlotObject, AnchorObject = anchorObject });
	}

	private ImageAnchorCollection? GetImageAnchorCollection(GameObject anchorGameObject) {
		foreach (var imageAnchor in _imageAnchorCollections) {
			if (imageAnchor.AnchorObject == anchorGameObject) return imageAnchor;
		}

		return null;
	}

	private void SubscribeToEvents()
	{
		_arTrackedImageManager.OnImageTracked += HandleAddedImages;
		_anchorManager.anchorsChanged += HandleAnchorsChanged;
	}

	private void UnsubscribeFromEvents()
	{
		_arTrackedImageManager.OnImageTracked -= HandleAddedImages;
		_anchorManager.anchorsChanged -= HandleAnchorsChanged;
	}

	private void OnDestroy()
	{
		UnsubscribeFromEvents();
	}
}
