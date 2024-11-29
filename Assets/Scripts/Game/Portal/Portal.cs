using UnityEngine;

public class Portal : MonoBehaviour
{
	[SerializeField] private Plot _plot;

    // Start is called before the first frame update
    void Start()
    {
		// ImageTrackingPlotUpdatedResponse.OnPlotDeactivated += HandlePlotDeactivated;
    }

	private void HandlePlotDeactivated(Plot plot)
	{
		if (plot != _plot) return;
		
		gameObject.SetActive(false);
	}

	private void OnDestroy()
	{
		// ImageTrackingPlotUpdatedResponse.OnPlotDeactivated -= HandlePlotDeactivated;
	}
}
