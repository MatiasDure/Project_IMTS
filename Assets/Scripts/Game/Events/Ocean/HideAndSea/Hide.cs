using UnityEngine;

public class Hide : MonoBehaviour
{
	[SerializeField] private Vector3 _hideOffset;
	private Transform _hideSpot;
    
    void Start()
    {
        HideAndSea.OnHideStart += HandleHideStart;
		HideAndSea.OnHideEnd += HandleHideEnd;
    }

    void Update()
    {
        if(_hideSpot == null) return;
    }

	private void HandleHideStart(UpdateBeeStateCollection hideMetadata) {
		_hideSpot = hideMetadata.Metadata.Target;
		HidePlayer();
	}

	private void HandleHideEnd(UpdateBeeStateCollection _) {
		_hideSpot = null;
	}

	private void HidePlayer() {
		transform.position = _hideSpot.position + _hideOffset;
	}
}
