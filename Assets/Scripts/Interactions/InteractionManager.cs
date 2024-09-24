using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
		// Raycast for 3d objects
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (
            Physics.Raycast(ray, out hit) &&
            hit.collider.TryGetComponent<Interactable>(out Interactable interactableObj)
        ) {
            interactableObj.OnInteraction();
		}
		
		// Raycast for UI elements
		PointerEventData pointerEventData = new PointerEventData(EventSystem.current)
		{
			position = Input.mousePosition
		};

		List<RaycastResult> results = new List<RaycastResult>();
		EventSystem.current.RaycastAll(pointerEventData, results);

		foreach (RaycastResult result in results)
		{
			if (result.gameObject.TryGetComponent<Interactable>(out Interactable uiInteractableObj))
			{
				uiInteractableObj.OnInteraction();
			}
		}
        
    }
}
