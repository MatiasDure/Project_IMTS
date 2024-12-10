using System;
using System.Collections;
using System.Collections.Generic;
using Codice.ThemeImages;
using UnityEditor;
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

	[Header("Settings")]
	[Tooltip("The boat speed movement.")]
	[SerializeField] private float _boatSpeed = .5f;

	[Tooltip("The initial scale of the boat.")]
	[SerializeField] private float _boatInitialScale = 0.03f;

	[Tooltip("Speed to scale up the boat.")]
	[SerializeField] private float _scaleUpSpeed = 1.2f;

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
		if (_waypoints.Length == 0) yield break;

		SpawnAtFirstPosition(_waypoints[0].Waypoint.position, _waypoints[0].Waypoint.forward);

		for (int i = 0; i < _waypoints.Length; i++)
		{
			BoatWaypoint waypoint = _waypoints[i];

			if (waypoint.IsWaypointInfrontBridge)
				MoveBeeToBridge();
			
			Vector3 waypointPosition = waypoint.Waypoint.position;

			if (i == 0) 
				StartCoroutine(ScaleUpBoat(waypointPosition, _waypoints[1].Waypoint.position));
			else if(i == _waypoints.Length - 1)
				StartCoroutine(ScaleDownBoat(waypointPosition, _waypoints[_waypoints.Length - 2].Waypoint.position));

			yield return StartCoroutine(_boatObject.RotateUntilLookAt(waypointPosition, .2f));
			yield return StartCoroutine(_boatObject.MoveUntilObjectReached(waypointPosition, _boatSpeed, 0.05f));
		}

		_boatObject.gameObject.SetActive(false);

		FireEndEvent(SetupEndEventMetadata());
	}

	private IEnumerator ScaleUpBoat(Vector3 firstWaypointPosition, Vector3 secondWaypointPosition) {
		while(_boatObject.transform.localScale.x < _boatInitialScale) {
			float distanceBetweenWaypoints = Vector3.Distance(firstWaypointPosition, secondWaypointPosition);
			float distanceToSecondWaypoint = Vector3.Distance(firstWaypointPosition, _boatObject.transform.position);
			float scale = distanceToSecondWaypoint / distanceBetweenWaypoints;

			_boatObject.transform.localScale = Vector3.one * scale * _boatInitialScale * _scaleUpSpeed;
			yield return null;
		}
	}

	private IEnumerator ScaleDownBoat(Vector3 lastWaypointPosition, Vector3 oneBeforeLastWaypointPosition) {
		while(_boatObject.transform.localScale.x > 0) {
			float distanceBetweenWaypoints = Vector3.Distance(lastWaypointPosition, oneBeforeLastWaypointPosition);
			float distanceToSecondWaypoint = Vector3.Distance(lastWaypointPosition, _boatObject.transform.position);
			float scale = distanceToSecondWaypoint / distanceBetweenWaypoints;

			_boatObject.transform.localScale = Vector3.one * scale * _boatInitialScale * _scaleUpSpeed;
			yield return null;
		}
	}

	private void MoveBeeToBridge()
	{
		Bee.Instance.UpdateState(BeeState.MovingToBridge);
		Vector3 bridgePosition = _bridgeTargetPosition.position;
		StartCoroutine(_beeMovement.RotateUntilLookAt(bridgePosition, .75f));
		StartCoroutine(_beeMovement.MoveUntilObjectReached(bridgePosition, .75f));
	}

	private void SpawnAtFirstPosition(Vector3 firstWaypointPosition, Vector3 firstWaypointForward)
	{
		_boatObject.gameObject.SetActive(true);
		_boatObject.transform.localScale = Vector3.zero;
		_boatObject.transform.SetPositionAndRotation(firstWaypointPosition, Quaternion.LookRotation(firstWaypointForward));
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
