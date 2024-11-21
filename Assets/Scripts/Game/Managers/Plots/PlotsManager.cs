using UnityEngine;

public class PlotsManager : Singleton<PlotsManager>
{
	const ushort AMOUNT_OF_PLOTS = 3;

	[SerializeField] internal Plot _currentPlot;
	[SerializeField] internal PlotCollection[] _plotObjects;

	public Plot CurrentPlot => _currentPlot;

    internal protected override void Awake() {
		base.Awake();
		if(_plotObjects.Length != AMOUNT_OF_PLOTS)
			Debug.LogWarning($"The amount of plot objects in the PlotsManager is not equal to the amount of plots in the game.\nThere should be {AMOUNT_OF_PLOTS} plot objects in the PlotsManager, but there are only {_plotObjects.Length} plot objects.");
	}

	private void Start()
	{
		ImageTrackingPlotActivatedResponse.OnPlotActivated += HandlePlotActivated;
	}

	private void HandlePlotActivated(Plot plot)
	{
		_currentPlot = plot;
		ActivateCurrentPlotObject(plot);
	}

	private void ActivateCurrentPlotObject(Plot plotToActivate)
	{
		foreach (var plotObject in _plotObjects)
		{
			if (plotObject.Plot != plotToActivate) continue;
			
			plotObject.PlotObject.SetActive(true);
			break;
		}
	}

	private void OnDestroy()
	{
		ImageTrackingPlotActivatedResponse.OnPlotActivated -= HandlePlotActivated;
	}
}
