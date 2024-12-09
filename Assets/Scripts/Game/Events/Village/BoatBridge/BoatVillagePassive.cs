using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatVillagePassive : PlotEvent, IInterruptible
{
	[Header("References")]
	[Tooltip("The bee companion prefab which contains the ObjectMovement component.")]
	[SerializeField] private ObjectMovement _beeMovement;
	
	[Tooltip("The boat movement.")]
	[SerializeField] private ObjectMovement _boat;

	[Tooltip("The waypoints the boat will follow.")]
	[SerializeField] private BoatWaypoint[] _waypoints;

	[Tooltip("The target position on the bridge for the bee to move to.")]
	[SerializeField] private Transform _bridgeTargetPosition;

	[Space]
	[Tooltip("The boat speed movement.")]
	[SerializeField] private float _boatSpeed = .5f;


	public event Action<IInterruptible> OnInterruptedDone;

	public override bool CanPlay() => true;

	public void InterruptEvent()
	{
		
	}

	public override void StartEvent()
	{
		base.StartEvent();
		UpdatePassiveEventCollection metadata = SetupStartEventMetadata();

		FireStartEvent(metadata);

		StartCoroutine(BoatPassiveEvent());
	}

	private IEnumerator BoatPassiveEvent()
	{
		foreach (BoatWaypoint waypoint in _waypoints)
		{
			// don't block the movement of the boat to move the bee towards the bridge
			if(waypoint.IsWaypointInfrontBridge) {
				Vector3 bridgePosition = _bridgeTargetPosition.position;
				StartCoroutine(_beeMovement.RotateUntilLookAt(bridgePosition, 1f));
				StartCoroutine(_boat.MoveUntilObjectReached(bridgePosition, 1f));
			}
			
			Vector3 waypointPosition = waypoint.Waypoint.position;

			yield return StartCoroutine(_boat.RotateUntilLookAt(waypointPosition, _boatSpeed));
			yield return StartCoroutine(_boat.MoveUntilObjectReached(waypointPosition, _boatSpeed));
		}

		FireEndEvent(SetupEndEventMetadata());
	}

	protected override void HandlePlotActivated()
	{
		if (PlotsManager.Instance.CurrentPlot != Plot.Village) return;

		SetUpPassiveEvent();
	}

	internal void SetUpPassiveEvent() {
		_state = EventState.InitialWaiting;
	}

	internal UpdatePassiveEventCollection SetupStartEventMetadata()
	{
		return new UpdatePassiveEventCollection
		{
			PreviousEvent = PassiveEventManager.Instance.CurrentEventPlaying,
			CurrentEvent = PassiveEvent.BeeVisit,
			State = BeeState.Inspecting,
		};
	}
}
