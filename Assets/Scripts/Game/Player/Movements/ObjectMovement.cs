using UnityEngine;

public class ObjectMovement : MonoBehaviour
{
	private const float DISTANCE_TOLERANCE = 0.2f;

	public void UpdatePosition(Vector3 position) {
		transform.position = position;
	}

	public void MoveTo(Vector3 position, float speed) {
		transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(position - transform.position), 100f * Time.deltaTime);
		transform.position = transform.forward * speed * Time.deltaTime + transform.position;
		// transform.position = Vector3.MoveTowards(transform.position, position, speed * Time.deltaTime);
	}

    public void SnapRotationTowards(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        if (direction == Vector3.zero) return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = targetRotation;
    }

    public void SmoothRotate(Quaternion startRotation, Quaternion targetRotation, float percentageCompleted)
    {
        transform.rotation = Quaternion.Slerp(startRotation, targetRotation, percentageCompleted);
    }

    public bool IsInPlace(Vector3 position) => ApproximatelyInPlace(position, DISTANCE_TOLERANCE);

	public bool ApproximatelyInPlace(Vector3 position, float tolerance) => Vector3.Distance(transform.position, position) <= tolerance;
}
