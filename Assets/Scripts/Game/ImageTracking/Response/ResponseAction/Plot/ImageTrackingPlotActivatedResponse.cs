using System;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ImageTrackingPlotActivatedResponse : MonoBehaviour, IImageTrackingResponse
{
	[SerializeField] private PlotTrackedImageCollection[] _plotTrackedImageCollections;
	[SerializeField] private AnchorManager _anchorManager;
	public ImageTrackingResponses ResponseType => ImageTrackingResponses.ActivatePlot;
	public static event Action<Plot> OnPlotActivated;

	private void Start() {
		_anchorManager.OnAnchorTracked += HandleAnchorActivated;
	}
	
	public GameObject Respond(GameObject portal, ARTrackedImage trackedImage)
	{
		_anchorManager.AttachToAnchor(portal, trackedImage);

		return portal;
	}

	private void HandleAnchorActivated(ImageAnchorCollection anchorCollection) {
		Plot plotActivated = GetPlot(anchorCollection.Image.referenceImage.name);

		OnPlotActivated?.Invoke(plotActivated);
	}

	private Plot GetPlot(string trackedImageName){
		foreach (var plotTrackedImageCollection in _plotTrackedImageCollections)
		{
			if (plotTrackedImageCollection.TrackedImageName != trackedImageName) continue;
			
			return plotTrackedImageCollection.Plot;
		}

		return Plot.None;
	}

	private void OnDestroy() {
		_anchorManager.OnAnchorTracked -= HandleAnchorActivated;
	}
}

[Serializable]
public struct PlotTrackedImageCollection{
	public string TrackedImageName;
	public Plot	Plot;
}

