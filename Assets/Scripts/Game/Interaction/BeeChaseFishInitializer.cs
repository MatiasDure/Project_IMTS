using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeChaseFishInitializer : InteractionBeeStateUpdate
{
    public override void Interact()
    {
        Debug.Log("Metadata interacted!");
        if (Bee.Instance.State != BeeState.Idle) return;

        UpdateInteractionStateCollection eventMetadata = new UpdateInteractionStateCollection();
        eventMetadata.State = BeeState.ChasingFish;
        eventMetadata.Metadata = new EventMetadata();
        eventMetadata.Metadata.Target = transform;

        base.FireInteractEvent(eventMetadata);
    }
}
