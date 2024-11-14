using UnityEngine;

public class ObjectMovement : MonoBehaviour
{
	private const float DISTANCE_TOLERANCE = 0.2f;

	public void UpdatePosition(Vector3 position) {
		transform.position = position;
	}

	public void MoveTo(Vector3 position, float speed) {
		transform.position = Vector3.MoveTowards(transform.position, position, speed * Time.deltaTime);
	}

	public bool IsInPlace(Vector3 position) => ApproximatelyInPlace(position, DISTANCE_TOLERANCE);

	public bool ApproximatelyInPlace(Vector3 position, float tolerance) => Vector3.Distance(transform.position, position) <= tolerance;
}
