using System;

[Serializable]
public class UpdateBeeStateCollection
{
	public BeeState State = BeeState.FollowingCamera;
	public EventMetadata Metadata = null;
}
