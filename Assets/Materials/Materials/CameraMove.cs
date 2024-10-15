using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.W))
		{
			transform.position += new Vector3(0, 0, 1f) * Time.deltaTime;
		}
		if(Input.GetKey(KeyCode.S))
		{
			transform.position += new Vector3(0, 0, -1f) * Time.deltaTime;
		}
		if(Input.GetKey(KeyCode.A))
		{
			transform.position += new Vector3(-1f, 0, 0) * Time.deltaTime;
		}
		if(Input.GetKey(KeyCode.D))
		{
			transform.position += new Vector3(1f, 0, 0) * Time.deltaTime;
		}
		if(Input.GetMouseButton(1)) {
			transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X"), 0));
		}
    }
}
