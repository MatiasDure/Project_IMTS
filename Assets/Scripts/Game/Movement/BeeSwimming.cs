using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeSwimming : BeeMovement
{
    [SerializeField] private Transform _middlePoint;

    //TEMP
    [SerializeField] private float _height;
    [SerializeField] private float _width;
    [SerializeField] private float _depth;

    private Coroutine _activeChangeDirectionCoroutine;
    private Coroutine _activeSmoothRotationCoroutine;

    private bool _checkHorizontalPosition = true;
    private bool _checkVerticalPosition = true;

    private float _rotationDuration;

    private void Start()
    {
        _rotationDuration = 1 / _rotationSpeed;
        _activeChangeDirectionCoroutine = StartCoroutine(ChangeDirectionCoroutine(GetRandomInterval()));
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
        Vector3 position = transform.position;
        Vector3 currentAngle = transform.eulerAngles;

        if (_checkHorizontalPosition && (ExceedsWidthBounds(position.x) || ExceedsDepthBounds(position.z)))
        {
            // Temporarily stop horizontal position checking, until rotation finishes
            StartCoroutine(DisableBooleanCoroutine(BooleansToDisable.HorizontalCheck, _rotationDuration + 1 / _rotationDuration));
            // Rotate bee
            SmoothRotateBeeQuaternion(GetLevelledRotationToMiddlePoint());
        }

        if (_checkVerticalPosition && ExceedsHeightBounds(position.y))
        {
            // Temporarily stop vertical position checking, until rotation finishes
            StartCoroutine(DisableBooleanCoroutine(BooleansToDisable.VerticalCheck, _rotationDuration + 1 / _rotationDuration));
            // Rotate bee
            SmoothRotateBeeVector(new Vector3(-currentAngle.x, currentAngle.y, currentAngle.z));
        }
    }

    // Smoothly rotate bee TO newAngle vector
    // Called when reaching bounds
    private void SmoothRotateBeeVector(Vector2 newAngle)
    {
        Quaternion targetRotation = Quaternion.Euler(newAngle);
        SmoothRotateBeeQuaternion(targetRotation);
    }

    // Smoothly rotate bee TO targetRotation quaternion
    // Called when reaching bounds
    private void SmoothRotateBeeQuaternion(Quaternion targetRotation)
    {
        // Stop previous rotation coroutine, if any
        StopSmoothRotationCoroutine();
        StopChangeDirectionCoroutine();
        _activeSmoothRotationCoroutine = StartCoroutine(SmoothRotationCoroutine(targetRotation, _rotationDuration));
    }

    // Smoothly rotate bee BY newAngles amount
    // Called when randomly changing direction
    private void SmoothRotateBeeByAngle(Vector2 newAngles)
    {
        Vector3 newRotationVec = new Vector3(newAngles.x, newAngles.y, 0);
        Quaternion targetRotation = Quaternion.Euler(transform.eulerAngles + newRotationVec);

        // Stop previous rotation coroutine, if any
        StopSmoothRotationCoroutine();

        _activeSmoothRotationCoroutine = StartCoroutine(SmoothRotationCoroutine(targetRotation, _rotationDuration));
    }

    IEnumerator ChangeDirectionCoroutine(float secondsToWait)
    {
        Vector2 newAngles = GetNewAngles();
        SmoothRotateBeeByAngle(newAngles);

        // Set Z to 0 due to orientation messing up
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x,
                                                 transform.localEulerAngles.y, 0);

        yield return new WaitForSeconds(secondsToWait);

        // Recursively call this coroutine
        _activeChangeDirectionCoroutine = StartCoroutine(ChangeDirectionCoroutine(GetRandomInterval()));
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

    IEnumerator DisableBooleanCoroutine(BooleansToDisable boolToDisable ,float disableDuration)
    {
        switch (boolToDisable)
        {
            case BooleansToDisable.HorizontalCheck:
                _checkHorizontalPosition = false;
                break;
            case BooleansToDisable.VerticalCheck:
                _checkVerticalPosition = false;
                break;
        }

        yield return new WaitForSeconds(disableDuration);

        switch (boolToDisable)
        {
            case BooleansToDisable.HorizontalCheck:
                _checkHorizontalPosition = true;
                break;
            case BooleansToDisable.VerticalCheck:
                _checkVerticalPosition = true;
                break;
        }
    }

    IEnumerator DisableGetDirectionCoroutine(float disableDuration)
    {
        StopCoroutine(_activeChangeDirectionCoroutine);

        yield return new WaitForSeconds(disableDuration);

        _activeChangeDirectionCoroutine = StartCoroutine(ChangeDirectionCoroutine(GetRandomInterval()));
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
            DisableGetDirectionCoroutine(_rotationDuration + GetRandomInterval());
        }
    }

    private Vector2 GetNewAngles() => new Vector2(GetNewRandomVerticalDirectionDegrees(), GetNewRandomHorizontalDirectionDegrees());

    // Get rotation that points to the middle point, with the same Y as the bee
    private Quaternion GetLevelledRotationToMiddlePoint()
    {
        // Set the middle point's Y the same as bee's Y
        Vector3 leveledMiddlePointPosition = _middlePoint.position;
        leveledMiddlePointPosition.y = transform.position.y;

        // Get direction vector to levelled middle point
        Vector3 directionToMiddlePoint = (leveledMiddlePointPosition - transform.position).normalized;

        // Get quaternion facing the middle point
        Quaternion levelledRotation = Quaternion.LookRotation(directionToMiddlePoint);

        return levelledRotation;
    }

    // For realistic movement
    private bool ExceedsVerticalAngleBounds(float updatedAngle) => updatedAngle >= _verticalRotationBound || (updatedAngle <= 360 - _verticalRotationBound);
    // Bounds
    private bool ExceedsWidthBounds(float x) => x > _middlePoint.position.x + _width / 2 || x < _middlePoint.position.x + -_width / 2;
    private bool ExceedsDepthBounds(float z) => z > _middlePoint.position.z + _depth / 2 || z < _middlePoint.position.z + -_depth / 2;
    private bool ExceedsHeightBounds(float y) => y > _middlePoint.position.y + _height / 2 || y < _middlePoint.position.y + -_height / 2;

    private float GetNewRandomHorizontalDirectionDegrees()
    {
        // Calculate and return new horizontal angle
        return Random.Range(-_horizontalTurnLimit, _horizontalTurnLimit);
    }

    private float GetNewRandomVerticalDirectionDegrees()
    {
        // Calculate new Vertical angle
        float currentAngle = transform.rotation.eulerAngles.x;
        float newAngle = Random.Range(-_verticalTurnLimit, _verticalTurnLimit);
        float updatedAngle = currentAngle + newAngle;

        // Flip angle if it exceeds the bounds to avoid unrealistic vertical movement
        updatedAngle = ExceedsVerticalAngleBounds(updatedAngle) ? -updatedAngle : updatedAngle;

        return updatedAngle;
    }

    // Random delay between each movement direction switch
    private float GetRandomInterval()
    {
        return Random.Range(_minDesicionSec, _maxDesicionSec);
    }

    enum BooleansToDisable
    {
        HorizontalCheck,
        VerticalCheck,
    }
}