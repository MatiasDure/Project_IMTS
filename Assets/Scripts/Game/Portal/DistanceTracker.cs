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
		CheckDistance();
    }

	private void HandleMaxDistanceReached() {
		OnMaxDistanceReached?.Invoke();
	}

	void CheckDistance()
	{
		if (!IsPlotActive()) return;

		if (Vector3.Distance(_object1.position, _object2.position) > _maxDistance)
		{
			HandleMaxDistanceReached();
		}
	}

	private bool IsPlotActive() => PlotsManager.Instance.CurrentPlot != Plot.None;

	public void UpdateObjects(Transform object1, Transform object2)
	{
		_object1 = object1;
		_object2 = object2;
	}

	public void UpdateMaxDistance(float maxDistance)
	{
		_maxDistance = maxDistance;
	}
}
