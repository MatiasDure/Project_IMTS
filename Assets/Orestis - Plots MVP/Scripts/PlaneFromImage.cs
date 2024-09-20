using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class PlaneFromImage : MonoBehaviour
{
    [SerializeField] private GameObject planePrefab;

    private GameObject spawnedPlane;


    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) DoRaycast();
    }

    private void OnImageClicked(GameObject imageGO)
    {

    }

    private void DoRaycast()
    {
        Vector3 mouseWorldPos;
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);

        RaycastHit hit;
        if(Physics.Raycast(mouseWorldPos, transform.TransformDirection(Vector3.forward),
                                                                   out hit, Mathf.Infinity))
        {
            if (hit.collider.CompareTag("ClickableImage")) OnImageClicked(hit.collider.gameObject);
        }
    }
}
