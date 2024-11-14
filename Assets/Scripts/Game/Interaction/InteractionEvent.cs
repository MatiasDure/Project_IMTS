using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class InteractionEvent : MonoBehaviour
{
    public static event Action<UpdateInteractionStateCollection> OnInteractionEventStart;
    public static event Action<UpdateInteractionStateCollection> OnInteractionEventEnd;
    protected void FireInteractEventStart(UpdateInteractionStateCollection eventMetadata)
    {
        OnInteractionEventStart?.Invoke(eventMetadata);
    }

    protected void FireInteractEventEnd(UpdateInteractionStateCollection eventMetadata)
    {
        OnInteractionEventEnd?.Invoke(eventMetadata);
    }
}