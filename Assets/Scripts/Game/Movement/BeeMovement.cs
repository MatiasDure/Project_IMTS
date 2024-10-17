using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotationSpeed = 1f;
    [SerializeField] private float _minDesicionSec;
    [SerializeField] private float _maxDesicionSec;
    [Range(0f, 90f)]
    [SerializeField] private float _horizontalTurnLimit;
    [SerializeField] private float _verticalTurnLimit = 3;
    [SerializeField] private float _verticalRotationBound = 10;
    [SerializeField] private Transform _middlePoint;

    private Coroutine _activeCoroutine;

    private void Start()
    {
        _activeCoroutine = StartCoroutine(ChangeDirectionCoroutine(GetRandomInterval()));
    }

    private void Update()
    {
        // Move
        transform.localPosition += transform.forward * _moveSpeed * Time.deltaTime;
    }

    IEnumerator ChangeDirectionCoroutine(float secondsToWait)
    {
        // Get new angles
        float newHorizontalDegrees = GetNewHorizontalDirectionDegrees();
        float newVerticalDegrees = GetNewVerticalDirectionDegrees();

        // Rotate
        Vector3 newRotationVec = new Vector3(newVerticalDegrees, newHorizontalDegrees, 0);
        transform.Rotate(newRotationVec, Space.Self);
        
        // Set Z to 0 due to orientation messing up (gimbal lock???)
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x,
                                                 transform.localEulerAngles.y, 0);

        yield return new WaitForSeconds(secondsToWait);

        // Recursively call this coroutine
        _activeCoroutine = StartCoroutine(ChangeDirectionCoroutine(GetRandomInterval()));
    }

    private float GetNewHorizontalDirectionDegrees()
    {
        // Calculate and return new horizontal angle
        return Random.Range(-_horizontalTurnLimit, _horizontalTurnLimit);
    }

    private float GetNewVerticalDirectionDegrees()
    {
        // Calculate new Vertical angle
        float currAngle = transform.rotation.eulerAngles.x;
        float newAngle = Random.Range(-_verticalTurnLimit, _verticalTurnLimit);
        float finalAngle = currAngle + newAngle;

        // Flip angle if it exceeds the bounds to avoid unrealistic vertical movement
        if(finalAngle >= _verticalRotationBound || (finalAngle <= 360 - _verticalRotationBound))
        {
            finalAngle = -finalAngle;
        }

        return finalAngle;
    }

    // Rotate towards the middle of the aquarium when hitting a bound
    private void ResetDirection()
    {
        // Get delta vec
        Vector3 directionToMiddle = _middlePoint.position - transform.position;
        
        // Rotate
        transform.rotation = Quaternion.LookRotation(directionToMiddle);

        // Activate coroutine again
        _activeCoroutine = StartCoroutine(ChangeDirectionCoroutine(GetRandomInterval()));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("OceanPlotBound"))
        {
            // Stop everything when hitting a wall and reset direction
            StopCoroutine(_activeCoroutine);
            ResetDirection();
        }
    }

    // Random delay between each movement direction switch
    private float GetRandomInterval()
    {
        return Random.Range(_minDesicionSec, _maxDesicionSec);
    }
}
