using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class InteractionBeeStateUpdate : MonoBehaviour, IInteractable
{
    public static event Action<UpdateInteractionStateCollection> OnInteract;
    public virtual void Interact() { }
    public bool CanInterrupt { get; set; }
    protected void FireInteractEvent(UpdateInteractionStateCollection eventMetadata)
    {
        OnInteract?.Invoke(eventMetadata);
    }
}
