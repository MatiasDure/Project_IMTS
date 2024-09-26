using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
	[SerializeField] private Transform _target;
	[SerializeField] private Vector3 _offset;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = _target.position + _offset;
    }
}
