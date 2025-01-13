using UnityEngine;

public class TutorialFishManager : MonoBehaviour
{
	[SerializeField] Fish _fishScript;
	[SerializeField] FishInteraction _fishInteractionScript;
	[SerializeField] TutorialFishMovement _tutorialFishMovementScript;
	[SerializeField] AvoidObjectSwimmingBehavior _avoidObstacleSwimmingBehavior;
	[SerializeField] AvoidObstacle _avoidObstacle ;
	
	private void Start()
	{
		_avoidObstacleSwimmingBehavior.enabled = false;
		_avoidObstacle.enabled = false;
	}
	
	private void EnableNormalFishMovement()
	{
		_fishScript.enabled = true;
		_fishInteractionScript.enabled = true;
		_avoidObstacleSwimmingBehavior.enabled = true;
		_avoidObstacle.enabled = true;
		
		_tutorialFishMovementScript.enabled = false;
		_fishInteractionScript.Interact();
	}
	
	public void EndTutorial()
	{
		EnableNormalFishMovement();
	}
}
