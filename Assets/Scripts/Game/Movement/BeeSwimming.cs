using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeSwimming : BeeMovement
{

    [SerializeField] private Transform _middlePoint;

    [SerializeField] private Vector3 _environmentDimensions;
    [SerializeField] private Vector2 _decisionTimeVector;
    [SerializeField] private Vector2 _turnLimitVector;

    [Tooltip("Limit rotation on the X axis (pitch) to prevent unrealistic movement")]
    [SerializeField] private float _verticalRotationBound = 10;

    private Coroutine _activeChangeDirectionCoroutine;
    private Coroutine _activeSmoothRotationCoroutine;

    private bool _checkHorizontalDirection = true;
    private bool _checkVerticalDirection = true;

    private float _rotationDuration;

    private void Start()
    {
        _rotationDuration = 1 / _rotationSpeed;
        _activeChangeDirectionCoroutine = StartCoroutine(ChangeDirectionCoroutine(GetRandomInterval(_decisionTimeVector)));
    }

    private void FixedUpdate()
    {
        Move();
        CheckPositionInBounds();
    }

    private void Move()
    {
        transform.position += transform.forward * _moveSpeed;
    }

    private void CheckPositionInBounds()
    {
        CheckHorizontalPosition(_environmentDimensions);
        CheckVerticalPosition(_environmentDimensions);
    }

    private void CheckHorizontalPosition(Vector3 dimensions)
    {
        if (!_checkHorizontalDirection) return;

        if (ExceedsWidthBounds(transform.position.x, _middlePoint.position.x, dimensions.x) ||
            ExceedsDepthBounds(transform.position.z, _middlePoint.position.z, dimensions.z))
        {
            // Temporarily stop horizontal position checking, until rotation finishes
            StartCoroutine(DisableDirectionCheckCoroutine(Directions.Horizontal, _rotationDuration + 1 / _rotationDuration));
            // Rotate bee
            SmoothRotateBeeQuaternion(GetLevelledRotationToMiddlePoint(_middlePoint.position), _rotationDuration);
        }
    }

    private void CheckVerticalPosition(Vector3 dimensions)
    {
        if (!_checkVerticalDirection) return;

        if (ExceedsHeightBounds(transform.position.y, _middlePoint.position.y, dimensions.y))
        {
            // Temporarily stop vertical position checking, until rotation finishes
            StartCoroutine(DisableDirectionCheckCoroutine(Directions.Vertical, _rotationDuration + 1 / _rotationDuration));
            // Rotate bee
            SmoothRotateBeeVector(new Vector3(-transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z), _rotationDuration);
        }
    }

    // Smoothly rotate bee TO newAngle vector
    // Called when reaching bounds
    private void SmoothRotateBeeVector(Vector2 newAngle, float rotationDuration)
    {
        Quaternion targetRotation = Quaternion.Euler(newAngle);
        SmoothRotateBeeQuaternion(targetRotation, rotationDuration);
    }

    // Smoothly rotate bee TO targetRotation quaternion
    // Called when reaching bounds
    private void SmoothRotateBeeQuaternion(Quaternion targetRotation, float rotationDuration)
    {
        // Stop previous rotation coroutine, if any
        StopSmoothRotationCoroutine();
        StopChangeDirectionCoroutine();
        _activeSmoothRotationCoroutine = StartCoroutine(SmoothRotationCoroutine(targetRotation, rotationDuration));
    }

    // Smoothly rotate bee BY newAngles amount
    // Called when randomly changing direction
    private void SmoothRotateBeeByAngle(Vector2 newAngles, float rotationDuration)
    {
        Vector3 newRotationVec = new Vector3(newAngles.x, newAngles.y, 0);
        Quaternion targetRotation = Quaternion.Euler(transform.eulerAngles + newRotationVec);

        // Stop previous rotation coroutine, if any
        StopSmoothRotationCoroutine();

        _activeSmoothRotationCoroutine = StartCoroutine(SmoothRotationCoroutine(targetRotation, rotationDuration));
    }

    IEnumerator ChangeDirectionCoroutine(float secondsToWait)
    {
        Vector2 newAngles = GetNewAngles();
        SmoothRotateBeeByAngle(newAngles, _rotationDuration);

        // Set Z to 0 due to orientation messing up
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x,
                                                 transform.localEulerAngles.y, 0);

        yield return new WaitForSeconds(secondsToWait);

        // Recursively call this coroutine
        _activeChangeDirectionCoroutine = StartCoroutine(ChangeDirectionCoroutine(GetRandomInterval(_decisionTimeVector)));
    }

    IEnumerator SmoothRotationCoroutine(Quaternion targetRotation, float duration)
    {
        Quaternion startRotation = transform.rotation;
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            // Slerp from start rotation to the target rotation
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // Ensure the final rotation is exactly the target rotation
        transform.rotation = targetRotation;
    }

    IEnumerator DisableDirectionCheckCoroutine(Directions directionToDisableChecking ,float disableDuration)
    {
        switch (directionToDisableChecking)
        {
            case Directions.Horizontal:
                _checkHorizontalDirection = false;
                break;
            case Directions.Vertical:
                _checkVerticalDirection = false;
                break;
            default:
                throw new System.Exception($"{GetType()}: {directionToDisableChecking} direction not handled.");
        }

        yield return new WaitForSeconds(disableDuration);

        switch (directionToDisableChecking)
        {
            case Directions.Horizontal:
                _checkHorizontalDirection = true;
                break;
            case Directions.Vertical:
                _checkVerticalDirection = true;
                break;
            default:
                throw new System.Exception($"{GetType()}: {directionToDisableChecking} direction not handled.");
        }
    }

    IEnumerator DisableGetDirectionCoroutine(float disableDuration)
    {
        StopCoroutine(_activeChangeDirectionCoroutine);

        yield return new WaitForSeconds(disableDuration);

        _activeChangeDirectionCoroutine = StartCoroutine(ChangeDirectionCoroutine(GetRandomInterval(_decisionTimeVector)));
    }

    private void StopSmoothRotationCoroutine()
    {
        if (_activeSmoothRotationCoroutine != null)
            StopCoroutine(_activeSmoothRotationCoroutine);
    }

    private void StopChangeDirectionCoroutine()
    {
        if (_activeChangeDirectionCoroutine != null)
        {
            DisableGetDirectionCoroutine(_rotationDuration + GetRandomInterval(_decisionTimeVector));
        }
    }

    private Vector2 GetNewAngles() => new Vector2(GetNewRandomVerticalDirectionDegrees(_turnLimitVector.y),
                                                  GetNewRandomHorizontalDirectionDegrees(_turnLimitVector.x));

    // Get rotation that points to the middle point, with the same Y as the bee
    private Quaternion GetLevelledRotationToMiddlePoint(Vector3 middlePoint)
    {
        // Set the middle point's Y the same as bee's Y
        Vector3 leveledMiddlePointPosition = middlePoint;
        leveledMiddlePointPosition.y = transform.position.y;

        // Get direction vector to levelled middle point
        Vector3 directionToMiddlePoint = (leveledMiddlePointPosition - transform.position).normalized;

        // Get quaternion facing the middle point
        Quaternion levelledRotation = Quaternion.LookRotation(directionToMiddlePoint);

        return levelledRotation;
    }

    // For realistic movement
    internal bool ExceedsVerticalAngleBound(float updatedAngle, float verticalRotationBound)
    {
        // Normalize the angle to be between -180 and 180 degrees
        float normalizedAngle = NormalizeAngle(updatedAngle);

        // Check if the angle is outside the vertical rotation bounds (-10 to 10)
        return normalizedAngle < -verticalRotationBound || normalizedAngle > verticalRotationBound;
    }
    // Bounds
    internal bool ExceedsWidthBounds(float beeX, float middlePointX, float boundsWidth) => beeX > middlePointX + boundsWidth / 2 || beeX < middlePointX + - boundsWidth/ 2;
    internal bool ExceedsDepthBounds(float beeZ, float middlePointZ, float boundsDepth) => beeZ > middlePointZ + boundsDepth / 2 || beeZ < middlePointZ + - boundsDepth / 2;
    internal bool ExceedsHeightBounds(float beeY, float middlePointY, float boundsHeight) => beeY > middlePointY + boundsHeight / 2 || beeY < middlePointY + - boundsHeight / 2;

    private float GetNewRandomHorizontalDirectionDegrees(float horizontalTurnLimit)
    {
        // Calculate and return new horizontal angle
        return Random.Range(-horizontalTurnLimit, horizontalTurnLimit);
    }

    private float GetNewRandomVerticalDirectionDegrees(float verticalTurnLimit)
    {
        // Calculate new Vertical angle
        float currentAngle = transform.rotation.eulerAngles.x;
        float newAngle = Random.Range(-verticalTurnLimit, verticalTurnLimit);
        float updatedAngle = currentAngle + newAngle;

        // Flip angle if it exceeds the bounds to avoid unrealistic vertical movement
        updatedAngle = ExceedsVerticalAngleBound(updatedAngle, _verticalRotationBound) ? -updatedAngle : updatedAngle;

        return updatedAngle;
    }

    // Random delay between each movement direction switch
    private float GetRandomInterval(Vector2 decisionTimeVector)
    {
        if (decisionTimeVector.x > decisionTimeVector.y)
            throw new System.Exception($"{GetType()}: DecisionTimeVector.x should be less than DecisionTimeVector.y");

        return Random.Range(decisionTimeVector.x, decisionTimeVector.y);
    }

    // Normalize the angle to be between -180 and 180 degrees
    private float NormalizeAngle(float angle)
    {
        angle = angle % 360; // Keep the angle between 0 and 360
        if (angle > 180) angle -= 360; // Convert to -180 to 180 range

        return angle;
    }

    enum Directions
    {
        None,
        Horizontal,
        Vertical,
    }
}