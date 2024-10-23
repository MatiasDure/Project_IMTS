using System;
using UnityEngine;

public class Hide : MonoBehaviour
{
	[SerializeField] private Vector3 _hideOffset;
	[SerializeField] private Movement _movement;
	private Transform _hideSpot;
	private bool _isMovingToHidingSpot = false;
    
	public static event Action OnHidedPlayer;

    void Start()
    {
		PlotEvent.OnPassiveEventStart += HandleHideStart;
		PlotEvent.OnPasiveEventEnd += HandleHideEnd;
        // HideAndSea.OnHideStart += HandleHideStart;
		// HideAndSea.OnHideEnd += HandleHideEnd;
    }

	void FixedUpdate() {
        if(_hideSpot == null || !_isMovingToHidingSpot) return;

		MoveTowardsHidingSpot();
		if(IsPlayerHidden())
			HidePlayer();
	}

	private void HandleHideStart(UpdatePassiveEventCollection eventMetadata) {
		if(eventMetadata.CurrentEvent != PassiveEvent.HideAndSea) return;

		_hideSpot = eventMetadata.Metadata.Target;
		_isMovingToHidingSpot = true;
	}

	private void HandleHideEnd(UpdatePassiveEventCollection eventMetadata) {
		if(eventMetadata.PreviousEvent != PassiveEvent.HideAndSea) return;

		_hideSpot = null;
	}

	private void MoveTowardsHidingSpot() {
		transform.position = Vector3.Lerp(transform.position, _hideSpot.position + _hideOffset, _movement.MovementSpeed * Time.deltaTime);
	}

	private bool IsPlayerHidden() => Vector3.Distance(transform.position, _hideSpot.position + _hideOffset) < 0.1f;

	private void HidePlayer() {
		_isMovingToHidingSpot = false;
		OnHidedPlayer?.Invoke();
	}

	private void SubscribeToEvents() {
		PlotEvent.OnPassiveEventStart += HandleHideStart;
		PlotEvent.OnPasiveEventEnd += HandleHideEnd;
	}

	private void UnsubscribeFromEvents() {
		PlotEvent.OnPassiveEventStart -= HandleHideStart;
		PlotEvent.OnPasiveEventEnd -= HandleHideEnd;
	}

	private void OnDestroy() {
		UnsubscribeFromEvents();
	}
}
