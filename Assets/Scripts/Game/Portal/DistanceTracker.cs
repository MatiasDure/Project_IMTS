using System;
using UnityEngine;

public class DistanceTracker : MonoBehaviour
{
	[SerializeField] private Transform _object1;
	[SerializeField] private Transform _object2;
	[SerializeField] private float _maxDistance;
	
	public event Action OnMaxDistanceReached;
    // Update is called once per frame
    void Update()
    {
		// if(HaveObjectsToTrack()) Debug.Log($"{_object1.name} and {_object2.name} can have only {_maxDistance} units apart and are currently {Vector3.Distance(_object1.position, _object2.position)} units apart");
		CheckDistance();
    }

	private void HandleMaxDistanceReached() {
		OnMaxDistanceReached?.Invoke();
		Debug.Log("Invoking max distance reached");
	}

	void CheckDistance()
	{
		if (!IsPlotActive() || !HaveObjectsToTrack()) return;

		if (Vector3.Distance(_object1.position, _object2.position) > _maxDistance)
		{
			HandleMaxDistanceReached();
		}
	}

	private bool HaveObjectsToTrack() => _object1 != null && _object2 != null;

	private bool IsPlotActive() => PlotsManager.Instance.CurrentPlot != Plot.None;

	public void UpdateObjects(Transform object1, Transform object2)
	{
		Debug.Log("Updated object");
		_object1 = object1;
		_object2 = object2;
	}

	public bool OverMaxDistance(Transform object1, Transform object2) => Vector3.Distance(object1.position, object2.position) > _maxDistance;

	public void UpdateMaxDistance(float maxDistance)
	{
		_maxDistance = maxDistance;
	}
}
