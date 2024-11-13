public enum ChestEventState
{
    None,
	OpeningChest, // close the chest,
	GoingInFrontChest, // release the bee and close the chest 
	InforntOfChest, // release the bee and close the chest
	GoingInsideChest, // release the bee and close the chest
	ClosingChest, // wait until close, open chest and release the bee
	InsideChest, // open chest and release the bee
	LeavingChest,
}
