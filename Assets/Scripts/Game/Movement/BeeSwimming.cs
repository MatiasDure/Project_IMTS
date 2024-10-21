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

    private Coroutine _activeCoroutine;

    private void Start()
    {
        _activeCoroutine = StartCoroutine(ChangeDirectionCoroutine(GetRandomInterval()));
    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Move();
        CheckPosition();
    }

    IEnumerator ChangeDirectionCoroutine(float secondsToWait)
    {
        Vector2 newAngles = GetNewAngles();
        RotateBeeByAngle(newAngles);

        // Set Z to 0 due to orientation messing up
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x,
                                                 transform.localEulerAngles.y, 0);

        yield return new WaitForSeconds(secondsToWait);

        // Recursively call this coroutine
        _activeCoroutine = StartCoroutine(ChangeDirectionCoroutine(GetRandomInterval()));
    }

    private void RotateBee(Vector3 newAngle)
    {
        transform.eulerAngles = newAngle;
    }

    private void RotateBeeByAngle(Vector2 newAngles)
    {
        Vector3 newRotationVec = new Vector3(newAngles.x, newAngles.y, 0);
        transform.Rotate(newRotationVec, Space.Self);
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
            RotateBee(new Vector3(currentAngle.x, currentAngle.y + 180, currentAngle.z));

        if(ExceedsHeightBounds(position.y))
            RotateBee(new Vector3(-currentAngle.x, currentAngle.y, currentAngle.z));
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