using UnityEngine;

public class ClamManager : Singleton<ClamManager>
{
	[SerializeField] private GameObject _clamsContainer;

	private IToggleComponent[] _clams;

	protected override void Awake() {
		base.Awake();
		_clams = _clamsContainer.GetComponentsInChildren<IToggleComponent>();
	}
    
	public bool TryGetRandomOpenClam(out IToggleComponent clamInteractioin) {
		foreach (IToggleComponent clam in _clams) {
			if(clam.CurrentToggleState != ToggleState.On) continue;

			clamInteractioin = clam;
			return true;
		}

		clamInteractioin = null;
		return false;
	}
}
