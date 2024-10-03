using UnityEngine;

public class FollowObject : MonoBehaviour
{
	[SerializeField] protected FollowConfiguration _followConfiguration;

	private void Awake() {
		if (_followConfiguration == null) throw new System.NullReferenceException("FollowObject: Follow configuration is not set");
		if (_followConfiguration.Target == null) throw new System.NullReferenceException("FollowObject: Target to follow is not set");
	}

	private void LateUpdate()
	{
		// Calling this in LateUpdate to make sure the target has moved before we move the object that follows
		Follow();

		if(_followConfiguration.LookAtTarget) transform.LookAt(_followConfiguration.Target);
	}

	protected virtual void Follow() 
	{
		Vector3 targetPosition = _followConfiguration.Target.position + _followConfiguration.Offset;
		float scaledSpeed = _followConfiguration.Speed * Time.deltaTime;
		transform.position = Vector3.Lerp(transform.position, targetPosition, scaledSpeed);
	}
}
