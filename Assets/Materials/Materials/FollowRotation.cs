using UnityEngine;

[RequireComponent(typeof(Camera))]
public class FollowRotation : MonoBehaviour
{
    private Quaternion initialRot;
	private Camera secondaryCamera;
	private Camera mainCamera;

	private bool _rotateRight = true;
    // Start is called before the first frame update
    void Start()
    {
        // Store the initial rotation offset
        initialRot = Quaternion.Euler(0, 0, 0);
		secondaryCamera = GetComponent<Camera>();
		mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        // Apply the initial rotation offset to the main camera's rotation
        // transform.rotation = Camera.main.transform.rotation * initialRot;
		SmoothRotate();
    }

	private void SmoothRotate() {
		if(_rotateRight) {
			transform.Rotate(Vector3.up, .05f);
			if(transform.rotation.y > 0.5f) {
				_rotateRight = false;
			}
		} else {
			transform.Rotate(Vector3.up, -.05f);
			if(transform.rotation.y < -0.5f) {
				_rotateRight = true;
			}
		}
	}
}