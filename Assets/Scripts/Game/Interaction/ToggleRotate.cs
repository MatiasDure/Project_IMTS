using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Toggle))]
public class ToggleRotate : MonoBehaviour, IToggleComponent
{
    public bool ignoreInput { get; set; }
    
    [SerializeField] private float _openAngle = 90.0f;
    [SerializeField] private float _rotateSpeed = 15f;

    [Serializable]
    private enum RotationAxis { x, y, z }
    
    [SerializeField] private RotationAxis _rotationAxis = RotationAxis.y;
    
    private Quaternion _closedRotation;
    private Quaternion _openRotation;
    
    public void Start()
    {
        SetRotation();
    }

    public void ToggleOn()
    {
        if(ignoreInput) return;
        SetRotation();
        StartCoroutine(Rotate(_openRotation));
    }

    public void ToggleOff()
    { 
        if((ignoreInput)) return;
        StartCoroutine(Rotate(_closedRotation));
    }

    private IEnumerator Rotate(Quaternion targetRotation)
    {
        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.01f)
        {
            ignoreInput = true;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotateSpeed * Time.deltaTime);
            yield return null; // Wait for the next frame
        }
        ignoreInput = false;
        transform.rotation = targetRotation;
    }

    private Quaternion GetOpenRotation(RotationAxis axis)
    {
        Vector3 rotationVector = _closedRotation.eulerAngles;

        switch (axis)
        {
            case RotationAxis.x:
                rotationVector.x += _openAngle;
                break;
            case RotationAxis.y:
                rotationVector.y += _openAngle;
                break;
            case RotationAxis.z:
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
