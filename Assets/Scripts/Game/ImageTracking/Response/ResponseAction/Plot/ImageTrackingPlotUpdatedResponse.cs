using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ImageTrackingPlotUpdatedResponse : MonoBehaviour, IImageTrackingResponse
{
	[SerializeField] private PlotTrackedImageCollection[] _plotTrackedImageCollections;
	[SerializeField] private PlotGameObjectCollection[] _plotGameObjectCollections;
	[SerializeField] private DistanceTracker _distanceTracker;

	private Dictionary<Plot, bool> _plotActiveState = new Dictionary<Plot, bool>();
	private Plot _currentPlotActive = Plot.None;

	public ImageTrackingResponses ResponseType => ImageTrackingResponses.UpdatePlot;
	public static event Action<Plot> OnPlotActivated;
	public static event Action<Plot> OnPlotDeactivated;

	private void Start() {
		_distanceTracker.OnMaxDistanceReached += HandleMaxDistanceReached;
	}

	public GameObject Respond(GameObject objectToManipulate, ARTrackedImage trackedImage)
	{
		Plot currentPlotUpdating = GetPlot(trackedImage.referenceImage.name);
		GameObject plotGameObject = GetGameObject(currentPlotUpdating);

		if(_distanceTracker.OverMaxDistance(plotGameObject.transform, Camera.main.transform)) return objectToManipulate;
		
		if(_currentPlotActive != currentPlotUpdating && _distanceTracker != null) {
			_currentPlotActive = currentPlotUpdating;
			_distanceTracker.UpdateObjects(plotGameObject.transform, Camera.main.transform);
		}

		if(trackedImage.trackingState == TrackingState.Tracking) 
			HandleTracking(plotGameObject, trackedImage);

		// HandleLimitedTracking(plotGameObject, trackedImage);
		return objectToManipulate;
	}

	private void HandleMaxDistanceReached() {
		GameObject plotGameObject = GetGameObject(_currentPlotActive);
		_plotActiveState[_currentPlotActive] = false;
		_currentPlotActive = Plot.None;
		plotGameObject.SetActive(false);
		Debug.Log("Plot deactivated");
		_distanceTracker.UpdateObjects(null, null);
		OnPlotDeactivated?.Invoke(_currentPlotActive);
	}

	private void HandleTracking(GameObject plotGameObject, ARTrackedImage trackedImage) {
		if(_plotActiveState.ContainsKey(_currentPlotActive) && _plotActiveState[_currentPlotActive]) {
			Debug.Log("Plot is already active");
			UpdatePlotPositionAndRotation(plotGameObject, trackedImage);
			return;
		}

		plotGameObject.SetActive(true);
		_plotActiveState[_currentPlotActive] = true;
		OnPlotActivated?.Invoke(_currentPlotActive);
		UpdatePlotPositionAndRotation(plotGameObject, trackedImage);
	}

	private void UpdatePlotPositionAndRotation(GameObject plotGameObject, ARTrackedImage trackedImage) {
		plotGameObject.transform.SetPositionAndRotation(trackedImage.transform.position, trackedImage.transform.rotation);
	}

	// private void HandleLimitedTracking(GameObject objectToManipulate, ARTrackedImage trackedImage) {
		
	// }

	private Plot GetPlot(string trackedImageName){
		foreach (var plotTrackedImageCollection in _plotTrackedImageCollections)
		{
			if (plotTrackedImageCollection.TrackedImageName != trackedImageName) continue;
			
			return plotTrackedImageCollection.Plot;
		}

		return Plot.None;
	}

	private GameObject GetGameObject(Plot plot){
		foreach (var plotGameObjectCollection in _plotGameObjectCollections)
		{
			if (plotGameObjectCollection.Plot != plot) continue;
			
			return plotGameObjectCollection.GameObject;
		}

		return null;
	}

	private void OnDestroy() {
		_distanceTracker.OnMaxDistanceReached -= HandleMaxDistanceReached;
	}
}

[Serializable]
public struct PlotGameObjectCollection
{
	public Plot Plot;
	public GameObject GameObject;
} 
