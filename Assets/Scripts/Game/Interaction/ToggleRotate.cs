using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Toggle))]
public class ToggleRotate : MonoBehaviour, IToggleComponent
{
    public ToggleState toggleState{ get; set; }

    public bool ignoreInput { get; set; }

    [SerializeField] internal float _openAngle = 90.0f;
    [SerializeField] internal float _rotateSpeed = 15f;

    [SerializeField] internal Axis _rotationAxis = Axis.Y;
    
    private Quaternion _closedRotation;
    private Quaternion _openRotation;

    public void Start()
    {
        toggleState = ToggleState.ToggleOff;
        SetRotationToToggelOn();
    }
    
    public void Toggle()
    {
        if(ignoreInput) return;
        
        //interact
        if (toggleState == ToggleState.ToggleOff) ToggleOn(); 
        else ToggleOff();
    }
    
    public void ToggleOn()
    {
        UpdateState(ToggleState.ToggleOn);
        SetRotationToToggelOn();
        StartCoroutine(RotateCoroutine(_openRotation));
    }

    public void ToggleOff()
    {
        UpdateState(ToggleState.ToggleOff);
        SetRotationToToggelOff();
        StartCoroutine(RotateCoroutine(_closedRotation));
    }
    private void UpdateState(ToggleState state)
    {
        toggleState = state;
    }
    private IEnumerator RotateCoroutine(Quaternion targetRotation)
    {
        ignoreInput = true;
        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.01f)
        {
            transform.rotation = RotateToTarget(transform.rotation, targetRotation, _rotateSpeed);
            yield return null; // Wait for the next frame
        }
        ignoreInput = false;
        //make sure the rotation reach to the target rotation
        transform.rotation = targetRotation;
    }

    internal Quaternion RotateToTarget(Quaternion origin, Quaternion target, float speed) =>
        Quaternion.RotateTowards(origin, target, speed * Time.deltaTime);

    internal Quaternion GetOpenRotation(Axis axis)
    {
        Vector3 rotationVector = _closedRotation.eulerAngles;

        switch (axis)
        {
            case Axis.X:
                rotationVector.x += _openAngle;
                break;
            case Axis.Y:
                rotationVector.y += _openAngle;
                break;
            case Axis.Z:
                rotationVector.z += _openAngle;
                break;
        }

        return Quaternion.Euler(rotationVector);
    }
    internal Quaternion GetCloseRotation(Axis axis)
    {
        Vector3 rotationVector = _openRotation.eulerAngles;

        switch (axis)
        {
            case Axis.X:
                rotationVector.x -= _openAngle;
                break;
            case Axis.Y:
                rotationVector.y -= _openAngle;
                break;
            case Axis.Z:
                rotationVector.z -= _openAngle;
                break;
        }

        return Quaternion.Euler(rotationVector);
    }

    private void SetRotationToToggelOn()
    {
        _closedRotation = transform.rotation;
        _openRotation = GetOpenRotation(_rotationAxis);
    }
    private void SetRotationToToggelOff()
    {
        _openRotation = transform.rotation;
        _closedRotation = GetCloseRotation(_rotationAxis);
    }
    
}
