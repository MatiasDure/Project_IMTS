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
    [SerializeField] private float _rotationStrength = 50;
    
    [Tooltip("Tornado pull force")]
    [SerializeField] private float _tornadoStrength = 2;

    private Rigidbody _rb;

    private List<CaughtObject> _caughtObjects = new List<CaughtObject>();
    
    public float strength => _rotationStrength;
    
    public Vector3 rotationAxis => _rotationAxis;

    public float rotationAngle => _rotationAngle;

    public float lift => _lift;

    // Start is called before the first frame update
    void Start()
    {
        SetUp();
    }

    private void SetUp()
    {
        //Normalize the rotation axis given by the user
        _rotationAxis.Normalize();

        _rb = GetComponent<Rigidbody>();
        _rb.isKinematic = true;
    }

    void FixedUpdate()
    {
        //try pull object in the center if it exceed maxdistance
        ApplyPullForce();
    }
    
    private void ApplyPullForce()
    {
        //Apply force to caught objects
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

    void OnTriggerEnter(Collider other)
    {
        if (!other.attachedRigidbody) return;
        if (other.attachedRigidbody.isKinematic) return;

        bool canBeCaught = other.TryGetComponent(out CaughtObject caught);
        
        if(!canBeCaught) return;
        
        InitCaughtObject(caught);
    }

    private void InitCaughtObject(CaughtObject caught)
    {
        caught.Init(this, _rb, _tornadoStrength);

        if (!_caughtObjects.Contains(caught)) _caughtObjects.Add(caught);
    }

    void OnTriggerExit(Collider other)
    {
        CaughtObject caught = other.GetComponent<CaughtObject>();
        
        if (caught) ReleaseObject(caught);
    }

    private void ReleaseObject(CaughtObject caught)
    {
        caught.Release();
        
        if (_caughtObjects.Contains(caught)) _caughtObjects.Remove(caught);
    }
}
