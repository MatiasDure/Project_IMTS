using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Toggle))]
public class ToggleRotate : MonoBehaviour, IToggleComponent
{
    internal ToggleState toggleState{ get; set; }

    ToggleState IToggleComponent.toggleState
    {
        get => toggleState;
        set => toggleState = value;
    }

    internal bool ignoreInput { get; set; }

    bool IToggleComponent.ignoreInput
    {
        get => ignoreInput;
        set => ignoreInput = value;
    }

    [SerializeField] internal float _openAngle = 90.0f;
    [SerializeField] internal float _rotateSpeed = 15f;

    [SerializeField] internal Axis _rotationAxis = Axis.Y;
    
    private Quaternion _closedRotation;
    private Quaternion _openRotation;

    public void Awake()
    {
        toggleState = ToggleState.ToggleOff;
    }

    public void Start()
    {
        SetRotation();
    }
    
    public void Toggle()
    {
        if(ignoreInput) return;
        //update state
        toggleState = toggleState == ToggleState.ToggleOff ? ToggleState.ToggleOn : ToggleState.ToggleOff;
        //interact
        if (toggleState == ToggleState.ToggleOn) ToggleOn(); 
        else ToggleOff();
    }
    
    public void ToggleOn()
    {
        SetRotation();
        StartCoroutine(RotateCoroutine(_openRotation));
    }

    public void ToggleOff()
    {
        StartCoroutine(RotateCoroutine(_closedRotation));
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

    private void SetRotation()
    {
        _closedRotation = transform.rotation;
        _openRotation = GetOpenRotation(_rotationAxis);
    }
    
}
