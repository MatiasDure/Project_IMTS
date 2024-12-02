using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiverSurfaceFishInteraction : MonoBehaviour, 
										   IEvent, 
										   IInteractable,
										   IInterruptible
{
	[SerializeField] GameObject fishPrefab;
	[SerializeField] float spawnFishDelay = 1f;
	[SerializeField] float spawnFishRandomAngleAmount = 15f;
	[SerializeField] Range spawnFishDistanceRange;
	[SerializeField] int maxFishCount = 3;
	[SerializeField] float sphereCastRadius = 5f;
	[SerializeField] float extendWaypointForwardDistance = 3f;
	
	public bool CanInterrupt { get;  set; } = true;
	public bool MultipleInteractions { get; set; } = false;
	public EventState State { get; set; } = EventState.None;
	public event Action OnEventDone;
	public event Action<IInterruptible> OnInterruptedDone;

	// The location in the river where the last action took place (Either spawned fish OR raycast hitpoint)
	private Vector3 lastRiverActionLocation;
	private int spawnedFish = 0;

	public void Interact()
	{
		if (State == EventState.Active) return;
		
		State = EventState.Active;
		
		lastRiverActionLocation = RaycastManager.Instance.HitPoint;
		StartCoroutine(DoSpawnFishSequence(spawnFishDelay));
	}
	
	private IEnumerator DoSpawnFishSequence(float spawnDelay)
	{
		List<RiverWaypoint> nearbyWaypoints = GetNearbyWaypoints(lastRiverActionLocation);
		RiverWaypoint targetWaypoint = GetFurthestDownstreamWaypoint(nearbyWaypoints);
		SpawnFish(targetWaypoint);

		yield return new WaitForSeconds(spawnDelay);
		
		if (spawnedFish >= maxFishCount) // Interaction done - reset
		{
			HandleEventDone();
			yield return null;
		}

		StartCoroutine(DoSpawnFishSequence(spawnDelay));
	}
	
	private void SpawnFish(RiverWaypoint targetWaypoint)
	{
		Vector3 randomRotationVector = 
			new Vector3(0, GetNewRandomHorizontalDirectionDegrees(spawnFishRandomAngleAmount), 0);
		Quaternion rotation = Quaternion.Euler(targetWaypoint.transform.eulerAngles + randomRotationVector);
		
		GameObject spawnedObject = Instantiate(fishPrefab, lastRiverActionLocation, rotation);
		
		if(spawnedFish != 0)
		{
			spawnedObject.transform.position += 
				spawnedObject.transform.forward * spawnFishDistanceRange.GetRandomValueWithinRange();
		}

		lastRiverActionLocation = spawnedObject.transform.position;
		spawnedFish++;
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

		return waypoints;
	}
	
	private RiverWaypoint GetFurthestDownstreamWaypoint(List<RiverWaypoint> waypoints)
	{
		float longestDistance = -1;
		RiverWaypoint targetWaypoint = null;

		for (int i = 0; i < waypoints.Count; i++)
		{
			float currentDistance = waypoints[i].GetExtendedForwardDistanceToPoint(lastRiverActionLocation, extendWaypointForwardDistance);
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
		return Physics.SphereCastAll(point, sphereCastRadius, Vector3.forward, 
		sphereCastRadius, Physics.AllLayers, QueryTriggerInteraction.Collide);
	}
	
	private float GetNewRandomHorizontalDirectionDegrees(float newAngleAmount)
	{
		return UnityEngine.Random.Range(-newAngleAmount, newAngleAmount);
	}
	
	private void HandleEventDone()
	{
		StopAllCoroutines();
		spawnedFish = 0;
		State = EventState.None;
		OnEventDone?.Invoke();
	}

	// No interruption needed - implemented to not break managers
	public void InterruptEvent()
	{
		OnInterruptedDone?.Invoke(this);
	}
}