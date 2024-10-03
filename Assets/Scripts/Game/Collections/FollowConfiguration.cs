using System;
using UnityEngine;

[Serializable]
public class FollowConfiguration
{
    [SerializeField] private Transform _target;
	[SerializeField] private float _speed = 1f;
	[SerializeField] private Vector3 _offset = new Vector3(0, 0, 0);
	[SerializeField] private bool _lookAtTarget = false;
	[SerializeField] private float _distance = 1f;

	public Transform Target => _target;
	public float Speed => _speed;
	public Vector3 Offset => _offset;
	public bool LookAtTarget => _lookAtTarget;
	public float Distance => _distance;
}
