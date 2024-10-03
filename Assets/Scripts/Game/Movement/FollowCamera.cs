using UnityEngine;

public class FollowCamera : FollowObject
{
	protected internal override void Follow() 
	{
		// Makes sure to stay in front of the Camera even if it rotates
		Vector3 targetPosition = _followConfiguration.Target.position + _followConfiguration.Target.forward * _followConfiguration.Distance + _followConfiguration.Offset;
		float scaledSpeed = _followConfiguration.Speed * Time.deltaTime;
		transform.position = Vector3.Lerp(transform.position, targetPosition, scaledSpeed);
	}
}
