using System;

[Serializable]
public class UpdatePassiveEventCollection
{
	public PassiveEvent PreviousEvent = PassiveEvent.None;
	public PassiveEvent CurrentEvent = PassiveEvent.None;
	public BeeState State = BeeState.Idle;
	public EventMetadata Metadata = null;
}
