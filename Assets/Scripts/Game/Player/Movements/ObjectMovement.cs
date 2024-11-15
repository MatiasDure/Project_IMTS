using UnityEngine;

public class ObjectMovement : MonoBehaviour
{
	private const float DEFAULT_DISTANCE_TOLERANCE = 0.2f;

	public void UpdatePosition(Vector3 position) {
		transform.position = position;
	}

	public void MoveTo(Vector3 position, float speed) {
		transform.position = Vector3.MoveTowards(transform.position, position, speed * Time.deltaTime);
	}

	public bool IsInPlace(Vector3 position, float distance = DEFAULT_DISTANCE_TOLERANCE) => ApproximatelyInPlace(position, distance);

	public bool ApproximatelyInPlace(Vector3 position, float tolerance) => Vector3.Distance(transform.position, position) <= tolerance;
}
