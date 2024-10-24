using UnityEngine;

public class PlotsManager : Singleton<PlotsManager>
{
	[SerializeField] internal Plot _currentPlot;

	public Plot CurrentPlot => _currentPlot;

    protected override void Awake() {
		base.Awake();
	}
}
