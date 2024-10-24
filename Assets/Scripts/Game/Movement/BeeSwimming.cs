using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeSwimming : BeeMovement
{
    [SerializeField] private Range _decisionDelayRange;
    [SerializeField] private Range _turnLimitRange;
    [SerializeField] private EnvironmentBounds bounds;
    [SerializeField] private Transform _middlePoint;

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
        bounds.SetCenter(_middlePoint.position);

        // Begin changing direction sequence
        _activeChangeDirectionCoroutine = 
            StartCoroutine(ChangeDirectionCoroutine(Range.GetRandomValueWithinRange(_decisionDelayRange.valuesRange)));
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
        CheckHorizontalPosition();
        CheckVerticalPosition();
    }

    private void CheckHorizontalPosition()
    {
        if (!_checkHorizontalDirection) return;

        if (bounds.ExceedsWidthBounds(transform.position.x) ||
            bounds.ExceedsDepthBounds(transform.position.z))
        {
            // Temporarily stop horizontal position checking, until rotation finishes
            StartCoroutine(DisableDirectionCheckCoroutine(Directions.Direction.Horizontal, _rotationDuration + 1 / _rotationDuration));
            // Rotate bee
            SmoothRotateBeeQuaternion(GetLevelledRotationToMiddlePoint(_middlePoint.position), _rotationDuration);
        }
    }

    private void CheckVerticalPosition()
    {
        if (!_checkVerticalDirection) return;

        if (bounds.ExceedsHeightBounds(transform.position.y))
        {
            // Temporarily stop vertical position checking, until rotation finishes
            StartCoroutine(DisableDirectionCheckCoroutine(Directions.Direction.Vertical, _rotationDuration + 1 / _rotationDuration));
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

    private void SmoothRotate(Quaternion startRotation, Quaternion targetRotation, float percentageCompleted)
    {
        transform.rotation = Quaternion.Slerp(startRotation, targetRotation, percentageCompleted);
    }

    private void ResetZAngle()
    {
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x,
                                                 transform.localEulerAngles.y, 0);
    }

    IEnumerator ChangeDirectionCoroutine(float secondsToWait)
    {
        Vector2 newAngles = GetNewAngles();
        SmoothRotateBeeByAngle(newAngles, _rotationDuration);
        ResetZAngle(); // Set Z to 0 due to orientation messing up

        yield return new WaitForSeconds(secondsToWait);

        // Recursively call this coroutine
        _activeChangeDirectionCoroutine = 
            StartCoroutine(ChangeDirectionCoroutine(Range.GetRandomValueWithinRange(_decisionDelayRange.valuesRange)));
    }

    IEnumerator SmoothRotationCoroutine(Quaternion targetRotation, float duration)
    {
        Quaternion startRotation = transform.rotation;
        float timeElapsed = 0f;
        float percentageCompleted;

        while (timeElapsed < duration)
        {
            // Slerp from start rotation to the target rotation
            percentageCompleted = timeElapsed / duration;
            SmoothRotate(startRotation, targetRotation, percentageCompleted);
            timeElapsed += Time.deltaTime;

            yield return null;
        }

        // Ensure the final rotation is exactly the target rotation
        transform.rotation = targetRotation;
    }

    IEnumerator DisableDirectionCheckCoroutine(Directions.Direction directionToDisableChecking ,float disableDuration)
    {
        UpdateDirectionChecks(directionToDisableChecking, false);

        yield return new WaitForSeconds(disableDuration);

        UpdateDirectionChecks(directionToDisableChecking, true);
    }

    IEnumerator DisableGetDirectionCoroutine(float disableDuration)
    {
        StopCoroutine(_activeChangeDirectionCoroutine);

        yield return new WaitForSeconds(disableDuration);

        _activeChangeDirectionCoroutine = 
            StartCoroutine(ChangeDirectionCoroutine(Range.GetRandomValueWithinRange(_decisionDelayRange.valuesRange)));
    }

    private void UpdateDirectionChecks(Directions.Direction directionToDisableChecking, bool updateValue)
    {
        switch (directionToDisableChecking)
        {
            case Directions.Direction.Horizontal:
                _checkHorizontalDirection = updateValue;
                break;
            case Directions.Direction.Vertical:
                _checkVerticalDirection = updateValue;
                break;
            default:
                throw new System.Exception($"{GetType()}: {directionToDisableChecking} direction not handled.");
        }
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
            DisableGetDirectionCoroutine(_rotationDuration + Range.GetRandomValueWithinRange(_decisionDelayRange.valuesRange));
        }
    }

    private Vector2 GetNewAngles() => new Vector2(GetNewRandomVerticalDirectionDegrees(_turnLimitRange.valuesRange.y),
                                                  GetNewRandomHorizontalDirectionDegrees(_turnLimitRange.valuesRange.x));

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
        float normalizedAngle = MathHelper.NormalizeAngle(updatedAngle);

        // Check if the angle is outside the vertical rotation bounds (-10 to 10)
        return normalizedAngle < -verticalRotationBound || normalizedAngle > verticalRotationBound;
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

    private float GetNewRandomHorizontalDirectionDegrees(float horizontalTurnLimit)
    {
        return Range.GetRandomValueNegativeToPositive(horizontalTurnLimit);
    }
}