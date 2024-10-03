using System;
using UnityEngine;

[Serializable]
public class FollowConfiguration
{
    [SerializeField] internal Transform _target;
	[SerializeField] internal float _speed = 1f;
	[SerializeField] internal Vector3 _offset = new Vector3(0, 0, 0);
	[SerializeField] internal bool _lookAtTarget = false;
	[SerializeField] internal float _distance = 1f;

	public Transform Target => _target;
	public float Speed => _speed;
	public Vector3 Offset => _offset;
	public bool LookAtTarget => _lookAtTarget;
	public float Distance => _distance;
}
