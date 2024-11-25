using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiverSurfaceInteraction : MonoBehaviour, 
                                       IEvent, 
                                       IInteractable
{
    public bool CanInterrupt {get;  set;} = false;
    public bool MultipleInteractions { get; set; } = false;
    public EventState State {get; set;} 

    public event Action OnEventDone;

    public void Interact()
    {
        //TODO: Check why this is not working
        Debug.Log("Interacted");
    }
}