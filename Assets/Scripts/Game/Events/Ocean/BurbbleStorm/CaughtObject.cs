using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaughtObject : MonoBehaviour
{
    private Tornado _tornadoReference;
    private SpringJoint _spring;
    
    [HideInInspector]
    public Rigidbody rigid;

    private void Awake()
    {
        enabled = false;
    }

    // Use this for initialization
    void Start()
    {
        SetUp();
    }

    private void SetUp()
    {
        rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //Lift spring so objects are pulled upwards
        if(_spring != null)
            LiftObject(_spring);
    }

    private void LiftObject(SpringJoint springJoint)
    {
        Vector3 newPosition = springJoint.connectedAnchor;
        newPosition.y = transform.position.y;
        springJoint.connectedAnchor = newPosition;
    }

    void FixedUpdate()
    {
        if(_tornadoReference != null)
            RotateObjectAroundTornado(_tornadoReference.transform.position,
                _tornadoReference.rotationAxis,
                _tornadoReference.rotationAngle,
                _tornadoReference.lift,
                _tornadoReference.strength);
    }

    private void RotateObjectAroundTornado(Vector3 tornadoCenter, Vector3 rotationAxis, float rotationAngle, 
                                                                                        float lift, float strength)
    {
        // Calculate the vector pointing from the tornado's center to the object's current position
        Vector3 direction = transform.position - _tornadoReference.transform.position;
    
        // Project this direction vector onto the plane defined by the tornado's rotation axis
        // This keeps the movement within the tornado's swirl plane
        Vector3 projection = Vector3.ProjectOnPlane(direction, _tornadoReference.rotationAxis);
    
        // Normalize the projection vector to make it a unit vector (direction only, no magnitude)
        projection.Normalize();
    
        // Rotate the normalized vector around the rotation axis by the specified rotation angle
        // This gives the swirl direction as if the object is moving around the tornado center
        Vector3 normal = Quaternion.AngleAxis(_tornadoReference.rotationAngle, _tornadoReference.rotationAxis) * projection;
    
        // Apply a secondary rotation to simulate "lift" in the direction of the swirl,
        // creating an upward (or downward) draft effect around the tornado
        normal = Quaternion.AngleAxis(_tornadoReference.lift, projection) * normal;
    
        // Add force in the direction of the calculated vector `normal`, scaled by the tornado's strength
        // This results in the object following a swirling, lifting motion
        rigid.AddForce(normal * _tornadoReference.strength, ForceMode.Force);
    }


    //Call this when tornadoReference already exists
    public void Init(Tornado tornadoRef, Rigidbody tornadoRigidbody, float springForce)
    {
        //Make sure this is enabled (for reentrance)
        //this bool stop the update if false
        enabled = true;
        
        //Save tornado reference
        _tornadoReference = tornadoRef;
        
        InitSpring(tornadoRigidbody, springForce);

        //Set initial position of the caught object relative to its position and the tornado
        InitCaughtObjectPosition();
    }

    private void InitCaughtObjectPosition()
    {
        Vector3 initialPosition = Vector3.zero;
        initialPosition.y = transform.position.y;
        _spring.connectedAnchor = initialPosition;
    }

    private void InitSpring(Rigidbody tornadoRigidbody, float springForce)
    {
        _spring = gameObject.AddComponent<SpringJoint>();
        _spring.spring = springForce;
        _spring.connectedBody = tornadoRigidbody;

        _spring.autoConfigureConnectedAnchor = false;
    }

    public void Release()
    {
        //stop the update when release
        enabled = false;
        Destroy(_spring);
    }
}
