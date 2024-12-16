using System;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ImageTrackingPlotActivatedResponse : MonoBehaviour, IImageTrackingResponse
{
	[SerializeField] private PlotTrackedImageCollection[] _plotTrackedImageCollections;
	[SerializeField] private AnchorManager _anchorManager;
	public ImageTrackingResponses ResponseType => ImageTrackingResponses.ActivatePlot;
	public static event Action<Plot> OnPlotActivated;

	public GameObject Respond(GameObject portal, ARTrackedImage trackedImage)
	{
		// portal.transform.SetPositionAndRotation(trackedImage.transform.position, trackedImage.transform.rotation);
		// portal.SetActive(true);
		// Plot plotActivated = GetPlot(trackedImage.referenceImage.name);
		// OnPlotActivated?.Invoke(plotActivated);

		return portal;
	}

	private void Start() {
		_anchorManager.OnAnchorTracked += HandleAnchorActivated;
	}

	private void HandleAnchorActivated(ImageAnchorCollection anchorCollection) {
		GameObject plotObject = anchorCollection.PlotObject;
		GameObject anchorObject = anchorCollection.AnchorObject;

		plotObject.transform.SetPositionAndRotation(anchorObject.transform.position, anchorObject.transform.rotation);
		plotObject.SetActive(true);

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

