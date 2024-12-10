using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTapObjectFollower : MonoBehaviour
{
	[SerializeField] Transform _objectToFollow;
	[SerializeField] Transform _camera;
	[SerializeField] float _distanceFromObject;
	
	private void Update()
	{
		FollowObject();
	}
	
	private void FollowObject()
	{
		Vector3 directionToCamera = (_camera.position - _objectToFollow.position).normalized;
		transform.position = _objectToFollow.position + directionToCamera * _distanceFromObject;
		transform.LookAt(_camera);
	}
}
