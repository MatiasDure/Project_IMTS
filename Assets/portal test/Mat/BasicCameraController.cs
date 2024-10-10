using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicCameraController : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed of camera movement
    public float lookSpeed = 2f; // Speed of camera rotation
    public float minY = -30f;     // Minimum vertical angle
    public float maxY = 60f;      // Maximum vertical angle

    private float rotationY = 0f; // Vertical rotation

    void Update()
    {
        // Camera movement
        float moveHorizontal = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float moveVertical = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

        Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);
        movement = transform.TransformDirection(movement); // Transform to world space
        transform.position += movement;

        // Camera rotation
        if (Input.GetMouseButton(1)) // Right mouse button held down
        {
            float mouseX = Input.GetAxis("Mouse X") * lookSpeed;
            float mouseY = Input.GetAxis("Mouse Y") * lookSpeed;

            rotationY -= mouseY;
            rotationY = Mathf.Clamp(rotationY, minY, maxY); // Clamp vertical rotation

            transform.localEulerAngles = new Vector3(rotationY, transform.localEulerAngles.y + mouseX, 0);
        }
    }
}
