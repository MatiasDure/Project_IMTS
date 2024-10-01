using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) DoRaycast();
    }

    private void DoRaycast()
    {
        Vector3 mousePos = Input.mousePosition;
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("Fish"))
            {
                SpeedUpFish(hit.collider.gameObject);
            }
        }
    }

    private void SpeedUpFish(GameObject fishGO)
    {
        FishAnimation fishScr = fishGO.transform.parent.GetComponent<FishAnimation>();
        fishScr.IncreaseSpeed();
    }
}