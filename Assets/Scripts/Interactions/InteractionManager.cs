using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            Raycast();
    }

    private void Raycast()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (
            Physics.Raycast(ray, out hit) &&
            hit.collider.TryGetComponent<Interactable>(out Interactable interactableObj)
        )
            interactableObj.OnInteraction();
        
    }
}
