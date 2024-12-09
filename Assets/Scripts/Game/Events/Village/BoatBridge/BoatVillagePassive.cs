using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatVillagePassive : PlotEvent, IInterruptible
{
	[Header("References")]
	[Tooltip("The bee companion prefab which contains the ObjectMovement component.")]
	[SerializeField] private ObjectMovement _beeMovement;
	
	[Tooltip("The boat object to activate and move.")]
	[SerializeField] private ObjectMovement _boatObject;

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
		OnInterruptedDone?.Invoke(this);
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
		if(_waypoints.Length == 0) yield break;

		Vector3 firstWaypoint = _waypoints[0].Waypoint.position;
		_boatObject.transform.position = firstWaypoint;
		_boatObject.transform.rotation = Quaternion.LookRotation(_waypoints[0].Waypoint.forward);

		foreach (BoatWaypoint waypoint in _waypoints)
		{
			// don't block the movement of the boat to move the bee towards the bridge
			if(waypoint.IsWaypointInfrontBridge) {
				Bee.Instance.UpdateState(BeeState.MovingToBridge);
				Vector3 bridgePosition = _bridgeTargetPosition.position;
				StartCoroutine(_beeMovement.RotateUntilLookAt(bridgePosition, .75f));
				StartCoroutine(_beeMovement.MoveUntilObjectReached(bridgePosition, .75f));
			}
			
			Vector3 waypointPosition = waypoint.Waypoint.position;

			yield return StartCoroutine(_boatObject.RotateUntilLookAt(waypointPosition, .2f));
			yield return StartCoroutine(_boatObject.MoveUntilObjectReached(waypointPosition, _boatSpeed, 0.05f));
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
			CurrentEvent = PassiveEvent.BoatTrip,
		};
	}
}
