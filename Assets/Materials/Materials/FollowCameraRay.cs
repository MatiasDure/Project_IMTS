// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class FollowCameraRay : MonoBehaviour
// {
// 	[SerializeField] private Camera secondaryCamera;
//     // Update is called once per frame
//     void Update()
//     {
//         if(Input.GetKeyDown(KeyCode.Space)) {
// 			OnMouseDown();
// 		}
//     }

// 	private void OnMouseDown()
// 	{
// 		// Create a ray from the secondary camera using the mouse position
// 		Ray mouseInWorld = secondaryCamera.ScreenPointToRay(Input.mousePosition);
// 		Debug.Log($"mouse pos: {Input.mousePosition}");
// 		Debug.Log($"mouse in world from secondary camera perspective: {mouseInWorld}");
// 		Debug.Log($"mouse in world from main camera perspective: {Camera.main.ScreenPointToRay(Input.mousePosition)}");

// 		// Get the point in the world space at a distance of 10 units from the camera
// 		Vector3 targetPosition = mouseInWorld.GetPoint(10f);

// 		Vector3 blah = secondaryCamera.transform.position + secondaryCamera.transform.forward * 10f;

// 		mouseInWorld = new Ray(mouseInWorld.origin,Camera.main.ScreenPointToRay(Input.mousePosition).direction);

// 		// Calculate the scaled movement
// 		// Vector3 scaledMovement = new Vector3(targetPosition.x * 0.4f, targetPosition.y * 0.4f, targetPosition.z);
// 		targetPosition = mouseInWorld.GetPoint(10f);

// 		// Apply the scaled movement to the object's position
// 		transform.position =  targetPosition;
// 	}
// }
using UnityEngine;

public class FollowCameraRay : MonoBehaviour
{
    [SerializeField] private Camera secondaryCamera;
    // [SerializeField] private Transform portalPlane;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnMouseDown();
        }
    }

    private void OnMouseDown()
    {
        // Create a ray from the main camera using the mouse position
        Ray mainCameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Perform the raycast from the main camera
        RaycastHit hit;
        if (Physics.Raycast(mainCameraRay, out hit, 100f))
        {
			Ray secondaryCameraRay =  new Ray(secondaryCamera.transform.position, mainCameraRay.direction);//secondaryCamera.ScreenPointToRay(secondaryCameraPoint);

			Vector3 targetPosition = secondaryCameraRay.GetPoint(10f);

			// Scale the movement
			Vector3 scaledMovement = targetPosition - secondaryCamera.transform.position;

			// Apply the scaled movement to the object's position
			transform.position = secondaryCamera.transform.position + scaledMovement;

			// if(Physics.Raycast(secondaryCameraRay, out RaycastHit hit2, 100f)) {
			// 	hit2.collider.TryGetComponent<Move>(out Move move);
			// 	if(move != null) {
			// 		move.StartFreezeMovement(2f);
			// 		Debug.Log("Freezing movement");
			// 	}
			// }
		}
    }
}