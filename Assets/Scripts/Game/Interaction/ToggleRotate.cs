using System;
using System.Collections;
using UnityEngine;

public class ToggleRotate : MonoBehaviour, IToggleComponent
{
    public ToggleState CurrentToggleState{ get; set; }
	/// <summary>
	/// UPDATE THIS: This boolean should be another state in the toggle state (something like switching)
	/// </summary>
    public bool ignoreInput { get; set; }
	public ToggleState NextToggleState { get; set; }

	[SerializeField] internal float _openAngle = 90.0f;
    [SerializeField] internal float _rotateSpeed = 15f;

    [SerializeField] internal Axis _rotationAxis = Axis.Y;
	[SerializeField] internal Transform _objectToRotate;
    
    private Quaternion _closedRotation;
    private Quaternion _openRotation;

	public event Action OnToggleDone;

	public void Start()
    {
        CurrentToggleState = ToggleState.Off;
    }
    
    public void Toggle()
    {
        if(CurrentToggleState == ToggleState.Switching) return;
        
        //interact
        if (CurrentToggleState == ToggleState.Off) ToggleOn(); 
        else ToggleOff();
    }
    
    public void ToggleOn()
    {
		NextToggleState = ToggleState.On;
        SetRotationToToggelOn();
        StartCoroutine(RotateCoroutine(_openRotation));
    }

    public void ToggleOff()
    {
		NextToggleState = ToggleState.Off;
        SetRotationToToggelOff();
        StartCoroutine(RotateCoroutine(_closedRotation));
    }
	
    private void UpdateState(ToggleState state)
	{
		CurrentToggleState = state;

		if (IsToggleDone(state))
			OnToggleDone?.Invoke();
	}

	private bool IsToggleDone(ToggleState state) => state != ToggleState.Switching;

	private IEnumerator RotateCoroutine(Quaternion targetRotation)
    {
        ignoreInput = true;
		UpdateState(ToggleState.Switching);
        while (Quaternion.Angle(_objectToRotate.rotation, targetRotation) > 0.01f)
        {
            _objectToRotate.rotation = RotateToTarget(_objectToRotate.rotation, targetRotation, _rotateSpeed);
            yield return null; // Wait for the next frame
        }
		UpdateState(NextToggleState);
        //make sure the rotation reach to the target rotation
        _objectToRotate.rotation = targetRotation;
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
        _closedRotation = _objectToRotate.rotation;
        _openRotation = GetOpenRotation(_rotationAxis);
    }
    private void SetRotationToToggelOff()
    {
        _openRotation = _objectToRotate.rotation;
        _closedRotation = GetCloseRotation(_rotationAxis);
    }
}
