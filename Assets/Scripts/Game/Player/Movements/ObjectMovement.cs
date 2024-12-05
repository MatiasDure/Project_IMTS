using System.Collections;
using UnityEngine;

public class ObjectMovement : MonoBehaviour
{
	[SerializeField] private bool _allowAvoidObject;
	
	private AvoidObstacle _avoidObstacle;
	private Vector3[] _directionsToCheck;
	private Vector3 _currentVelocity;

	private readonly int _directionCount = 20;
	private readonly float _obstacleDistance = 1f;
	private readonly float _smoothTime = 0.5f;
	
	private const float DISTANCE_TOLERANCE = 0.2f;

	private void Awake()
	{
		if (_allowAvoidObject) _avoidObstacle = gameObject.AddComponent<AvoidObstacle>();
	}

	private void Start()
	{
		if (_allowAvoidObject) _directionsToCheck = _avoidObstacle.CalculateDirections(_directionCount);
	}

	public void UpdatePosition(Vector3 position) {
		transform.position = position;
	}

	public void MoveTo(Vector3 position, float speed)
	{
		Vector3 moveVector = CalculateMovement(position, speed);

		SmootherAndApplyMovement(speed, moveVector);
	}

	private Vector3 CalculateMovement(Vector3 position, float speed)
	{
		Vector3 targetVector = position - transform.position;
		Vector3 avoidVector = Vector3.zero;

		if (_allowAvoidObject) 
			avoidVector = _avoidObstacle.CalculateObstacleAvoidance(speed, transform, _obstacleDistance, _directionsToCheck);

		Vector3 moveVector = targetVector + avoidVector;
		
		return moveVector;
	}

	private void SmootherAndApplyMovement(float speed, Vector3 moveVector)
	{
		moveVector = Vector3.SmoothDamp(transform.forward, moveVector, ref _currentVelocity, _smoothTime);
		moveVector = moveVector.normalized * speed;

		transform.forward = moveVector;

		transform.position += moveVector * Time.deltaTime;
	}

	public void SnapRotationTowards(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        if (direction == Vector3.zero) return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = targetRotation;
    }

	public IEnumerator MoveUntilObjectReached(Vector3 targetPosition, float speed) {
		while (!IsInPlace(targetPosition)) {
			MoveTo(targetPosition, speed);
			yield return null;
		}
	}

    public void SmoothRotate(Quaternion startRotation, Quaternion targetRotation, float percentageCompleted)
    {
        transform.rotation = Quaternion.Slerp(startRotation, targetRotation, percentageCompleted);
    }

    public bool IsInPlace(Vector3 position) => ApproximatelyInPlace(position, DISTANCE_TOLERANCE);

	public bool ApproximatelyInPlace(Vector3 position, float tolerance) => Vector3.Distance(transform.position, position) <= tolerance;
}
