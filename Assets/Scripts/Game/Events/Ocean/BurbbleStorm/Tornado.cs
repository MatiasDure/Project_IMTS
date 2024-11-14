using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Tornado : MonoBehaviour
{
    [Tooltip("Distance after which the rotation physics starts")]
    [SerializeField] private float _maxDistance = 20;
    
    [Tooltip("The axis that the caught objects will rotate around")]
    [SerializeField] private Vector3 _rotationAxis = new Vector3(0, 1, 0);
    
    [Tooltip("The angle will be use to determine the direction and rotation angle of the caught oject")]
    public float _rotationAngle = 130f;
    
    [Tooltip("Angle that is added to the object's velocity (higher lift -> quicker on top)")]
    [Range(0, 90)]
    [SerializeField] private float _lift = 45;

    [Tooltip("The force that will drive the caught objects around the tornado's center")]
    [SerializeField] private float _rotationStrength = 1;

    [Tooltip("Tornado pull force")]
    [SerializeField] private float _tornadoStrength = 10;

    private Rigidbody _rb;

    private List<CaughtObject> _caughtObjects = new List<CaughtObject>();
    
    public float strength => _rotationStrength;
    
    public Vector3 rotationAxis => _rotationAxis;

    public float rotationAngle => _rotationAngle;

    public float lift => _lift;

    void Awake()
    {
        SetUp();
    }

    private void SetUp()
    {
        _rotationAxis.Normalize();

        _rb = GetComponent<Rigidbody>();
        _rb.isKinematic = true;
    }

    void FixedUpdate()
    {
        //try pull object in the center if it exceed maxdistance
        ApplyForce();
    }
    
    private void ApplyForce()
    {
        for (int i = 0; i < _caughtObjects.Count; i++)
        {
            if (_caughtObjects[i] != null)
            {
                Vector3 pull = transform.position - _caughtObjects[i].transform.position;
                
                if (pull.magnitude > _maxDistance)
                    ApplyPullForce(_caughtObjects[i], pull);
                else 
                    _caughtObjects[i].enabled = true;
            }
        }
    }

    private void ApplyPullForce(CaughtObject caughtObject, Vector3 pull)
    {
        caughtObject.rigid.AddForce(pull.normalized * (pull.magnitude * _tornadoStrength), ForceMode.Force);
        caughtObject.enabled = false;
    }

    private void OnEnable()
    {
        _caughtObjects = new List<CaughtObject>(FindObjectsOfType<CaughtObject>());

        foreach (var caught in _caughtObjects)
        {
            if (caught != null) caught.Init(this, _rb, _tornadoStrength);
        }
    }

    private void OnDisable()
    {

        foreach (var caught in _caughtObjects)
        {
            if (caught != null) caught.Release();
        }

        _caughtObjects.Clear();
    }

}
