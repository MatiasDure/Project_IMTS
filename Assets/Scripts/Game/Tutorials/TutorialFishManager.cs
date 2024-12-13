using UnityEngine;

public class TutorialFishManager : MonoBehaviour
{
	[SerializeField] Fish _fishScript;
	[SerializeField] FishInteraction _fishInteractionScript;
	[SerializeField] TutorialFishMovement _tutorialFishMovementScript;
	
	private void EnableNormalFishMovement()
	{
		_fishScript.enabled = true;
		_fishInteractionScript.enabled = true;
		
		_tutorialFishMovementScript.enabled = false;
		_fishInteractionScript.Interact();
	}
	
	public void EndTutorial()
	{
		EnableNormalFishMovement();
	}
}
