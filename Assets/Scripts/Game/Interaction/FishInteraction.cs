using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishInteraction : InteractionBeeStateUpdate
{
    public override void Interact()
    {
        if (Bee.Instance.State != BeeState.Idle) return;

        UpdateInteractionStateCollection eventMetadata = new UpdateInteractionStateCollection();
        eventMetadata.State = BeeState.CatchingFish;
        eventMetadata.Metadata.Target = transform;
    }
}
