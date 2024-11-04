using System;

[Serializable]
public class UpdateInteractionStateCollection
{
    public BeeState State = BeeState.Idle;
    public EventMetadata Metadata = null;
}
