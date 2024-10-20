using UnityEngine;

public class PlotsManager : Singleton<PlotsManager>
{
	[SerializeField] private Plot _currentPlot;

	public Plot CurrentPlot => _currentPlot;

    protected override void Awake() {
		base.Awake();
	}
}
