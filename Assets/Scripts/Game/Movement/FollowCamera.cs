using UnityEngine;

public class FollowCamera : FollowObject
{
	bool _isFollowing = true;

	private void Start() {
		Bee.OnBeeStateChanged += HandleBeeStateChange;
	}

	protected internal override void Follow() 
	{
		if (!_isFollowing) return;

		// Makes sure to stay in front of the Camera even if it rotates
		Vector3 targetPosition = _followConfiguration.Target.position + _followConfiguration.Target.forward * _followConfiguration.Distance + _followConfiguration.Offset;
		float scaledSpeed = _followConfiguration.Speed * Time.deltaTime;
		transform.position = Vector3.Lerp(transform.position, targetPosition, scaledSpeed);
	}

	private void HandleBeeStateChange(BeeState state) {		
		_isFollowing = state == BeeState.FollowingCamera;
	}

	private void OnDestory() {
		Bee.OnBeeStateChanged -= HandleBeeStateChange;
	}
}
