using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class TutorialFishManager : MonoBehaviour, IInteractable
{
	public bool CanInterrupt { get; set; } = false;
	public bool MultipleInteractions { get; set; } = false;

	[SerializeField] Fish _fishScript;
	[SerializeField] FishInteraction _fishInteractionScript;
	[SerializeField] TutorialFishMovement _tutorialFishMovementScript;

	public void Interact()
	{
		EnableNormalFishMovement();
	}
	
	private void EnableNormalFishMovement()
	{
		_fishScript.enabled = true;
		_fishInteractionScript.enabled = true;
		
		_tutorialFishMovementScript.enabled = false;
		_fishInteractionScript.Interact();
	}
	
	//TODO: Call from PlotTutorial when time runs out or smth
	private void EndTutorial()
	{
		EnableNormalFishMovement();
	}
}
