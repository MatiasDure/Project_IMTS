using System;

[Serializable]
public class UpdatePassiveEventCollection
{
	public PassiveEvent PreviousEvent = PassiveEvent.None;
	public PassiveEvent CurrentEvent = PassiveEvent.None;
	public BeeState State = BeeState.FollowingCamera;
	public EventMetadata Metadata = null;
}
