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

    private Coroutine _activeGetDirectionCoroutine;
    private Coroutine _activeSmoothRotationCoroutine;

    private void Start()
    {
        _activeGetDirectionCoroutine = StartCoroutine(ChangeDirectionCoroutine(GetRandomInterval()));
    }

    private void FixedUpdate()
    {
        Move();
        CheckPosition();
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
        _activeGetDirectionCoroutine = StartCoroutine(ChangeDirectionCoroutine(GetRandomInterval()));
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

    private void SmoothRotateBee(Vector2 newAngle)
    {
        Quaternion targetRotation = Quaternion.Euler(newAngle);

        // Stop previous rotation coroutine, if any
        if (_activeSmoothRotationCoroutine != null)
            StopCoroutine(_activeSmoothRotationCoroutine);

        _activeSmoothRotationCoroutine = StartCoroutine(SmoothRotationCoroutine(targetRotation, 0f));
    }

    private void SmoothRotateBeeByAngle(Vector2 newAngles)
    {
        Vector3 newRotationVec = new Vector3(newAngles.x, newAngles.y, 0);
        Quaternion targetRotation = Quaternion.Euler(transform.eulerAngles + newRotationVec);

        // Stop previous rotation coroutine, if any
        if (_activeSmoothRotationCoroutine != null)
            StopCoroutine(_activeSmoothRotationCoroutine);

        _activeSmoothRotationCoroutine = StartCoroutine(SmoothRotationCoroutine(targetRotation, 1 / _rotationSpeed));
    }

    private Vector2 GetNewAngles() => new Vector2(GetNewVerticalDirectionDegrees(), GetNewHorizontalDirectionDegrees());

    private float GetNewHorizontalDirectionDegrees()
    {
        // Calculate and return new horizontal angle
        return Random.Range(-_horizontalTurnLimit, _horizontalTurnLimit);
    }

    private float GetNewVerticalDirectionDegrees()
    {
        // Calculate new Vertical angle
        float currentAngle = transform.rotation.eulerAngles.x;
        float newAngle = Random.Range(-_verticalTurnLimit, _verticalTurnLimit);
        float updatedAngle = currentAngle + newAngle;

        // Flip angle if it exceeds the bounds to avoid unrealistic vertical movement
        updatedAngle = ExceedsVerticalAngleBounds(updatedAngle) ? -updatedAngle : updatedAngle ;

        return updatedAngle;
    }

    private bool ExceedsVerticalAngleBounds(float updatedAngle) => updatedAngle >= _verticalRotationBound || (updatedAngle <= 360 - _verticalRotationBound);

    private void Move()
    {
        transform.position += transform.forward * _moveSpeed;
    }

    private void CheckPosition()
    {
        Vector3 position = transform.position;
        Vector3 currentAngle = transform.eulerAngles;

        if (ExceedsWidthBounds(position.x) || ExceedsDepthBounds(position.z))
            SmoothRotateBee(new Vector3(currentAngle.x, currentAngle.y + 180, currentAngle.z));

        if(ExceedsHeightBounds(position.y))
            SmoothRotateBee(new Vector3(-currentAngle.x, currentAngle.y, currentAngle.z));
    }

    private bool ExceedsWidthBounds(float x) => x > _middlePoint.position.x + _width / 2 || x < _middlePoint.position.x + -_width / 2;
    private bool ExceedsDepthBounds(float z) => z > _middlePoint.position.z + _depth / 2 || z < _middlePoint.position.z + -_depth / 2;
    private bool ExceedsHeightBounds(float y) => y > _middlePoint.position.y + _depth / 2 || y < _middlePoint.position.y + -_height / 2;

    // Random delay between each movement direction switch
    private float GetRandomInterval()
    {
        return Random.Range(_minDesicionSec, _maxDesicionSec);
    }
}