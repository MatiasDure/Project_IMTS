using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiverSurfaceFishInteraction : MonoBehaviour, 
										   IEvent, 
										   IInteractable,
										   IInterruptible
{
	[SerializeField] GameObject _fishPrefab;
	[SerializeField] float _spawnFishRandomAngleAmount = 15f;
	[SerializeField] int _maxSequencePlaysAmount = 3;
	[SerializeField] float _sphereCastRadius = 5f;
	[SerializeField] float _extendWaypointForwardDistance = 3f;
	[SerializeField] float _hitpointYOffset = 1f;
	
	public bool CanInterrupt { get;  set; } = true;
	public bool MultipleInteractions { get; set; } = false;
	public EventState State { get; set; } = EventState.None;
	public event Action OnEventDone;
	public event Action<IInterruptible> OnInterruptedDone;

	// The location in the river where the last action took place (Either spawned fish OR raycast hitpoint)
	private Vector3 _lastRiverActionLocation;
	private int _sequencePlayedAmount = 0;
	private RiverFish _spawnedRiverFish;

	public void Interact()
	{
		if (State == EventState.Active) return;
		
		State = EventState.Active;
		
		_lastRiverActionLocation = RaycastManager.Instance.HitPoint;
		_lastRiverActionLocation.y -= _hitpointYOffset;
		DoSpawnFishSequence();
	}
	
	private void DoSpawnFishSequence()
	{
		List<RiverWaypoint> nearbyWaypoints = GetNearbyWaypoints(_lastRiverActionLocation);
		RiverWaypoint targetWaypoint = GetFurthestDownstreamWaypoint(nearbyWaypoints);
		SpawnFish(targetWaypoint);
	}

	private void SpawnFish(RiverWaypoint targetWaypoint)
	{
		Vector3 randomRotationVector =
			new Vector3(0, GetNewRandomHorizontalDirectionDegrees(_spawnFishRandomAngleAmount), 0);
		Quaternion rotation = Quaternion.Euler(targetWaypoint.transform.eulerAngles + randomRotationVector);

		GameObject spawnedObject = Instantiate(_fishPrefab, _lastRiverActionLocation, rotation);
		_spawnedRiverFish = spawnedObject.GetComponent<RiverFish>();
		SubscribeToSpawnedFish();

		_sequencePlayedAmount++;
	}
	
	
	private void HandleFishAnimationDone(RiverFish riverFish)
	{	
		if (_sequencePlayedAmount >= _maxSequencePlaysAmount) // Interaction done - reset
		{		
			HandleEventDone();
			return;
		}
		
		_lastRiverActionLocation = riverFish.transform.position;
		Destroy(_spawnedRiverFish.gameObject);
		DoSpawnFishSequence();
	}

	private List<RiverWaypoint> GetNearbyWaypoints(Vector3 point)
	{
		RaycastHit[] hitArray = GetSphereCastCollisions(point);
		List<RiverWaypoint> waypoints = new List<RiverWaypoint>();
		
		RiverWaypoint waypoint;
		for (int i = 0; i < hitArray.Length; i++)
		{
			if (hitArray[i].collider.gameObject.TryGetComponent<RiverWaypoint>(out waypoint))
			{
				waypoints.Add(waypoint);
			}
		}
		Debug.Log(waypoints.Count);
		return waypoints;
	}
	
	private RiverWaypoint GetFurthestDownstreamWaypoint(List<RiverWaypoint> waypoints)
	{
		float longestDistance = -1;
		RiverWaypoint targetWaypoint = null;

		for (int i = 0; i < waypoints.Count; i++)
		{
			float currentDistance = waypoints[i].GetExtendedForwardDistanceToPoint(_lastRiverActionLocation, _extendWaypointForwardDistance);
			if (currentDistance >= longestDistance)
			{
				longestDistance = currentDistance;
				targetWaypoint = waypoints[i];
			}
		}

		return targetWaypoint;
	}
		
	private RaycastHit[] GetSphereCastCollisions(Vector3 point)
	{
		return Physics.SphereCastAll(point, _sphereCastRadius, Vector3.forward, 
		_sphereCastRadius, Physics.AllLayers, QueryTriggerInteraction.Collide);
	}
	
	private float GetNewRandomHorizontalDirectionDegrees(float newAngleAmount)
	{
		return UnityEngine.Random.Range(-newAngleAmount, newAngleAmount);
	}
	
	private void HandleEventDone()
	{
		UnsubscriveFromSpawnedFish();
		Destroy(_spawnedRiverFish.gameObject);
		_sequencePlayedAmount = 0;
		State = EventState.None;
		OnEventDone?.Invoke();
	}
	
	private void SubscribeToSpawnedFish()
	{
		_spawnedRiverFish.OnAnimationFinished += HandleFishAnimationDone;
	}
	
	private void UnsubscriveFromSpawnedFish()
	{
		_spawnedRiverFish.OnAnimationFinished -= HandleFishAnimationDone;
	}

	// No interruption needed - implemented to not break managers
	public void InterruptEvent()
	{
		OnInterruptedDone?.Invoke(this);
	}

	public void StopEvent()
	{
		OnEventDone?.Invoke();
	}
}