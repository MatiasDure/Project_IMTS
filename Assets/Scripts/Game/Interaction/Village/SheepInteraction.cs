using System;
using System.Collections;
using UnityEngine;

[
	RequireComponent(typeof(BoxCollider)),
	RequireComponent(typeof(PlayAnimation)),
	RequireComponent(typeof(AudioSource)),
]
public class SheepInteraction : MonoBehaviour, IInteractable, IEvent, IInterruptible
{
	[SerializeField] private ObjectMovement _beeMovement;
	
	private PlayAnimation _beeAnimation;
	private AudioSource _audioSource;

	public bool CanInterrupt { get; set; }
	public bool MultipleInteractions { get; set; }
	public EventState State { get; set; }

	public event Action<IInterruptible> OnInterruptedDone;
	public event Action OnEventDone;

	public void Interact()
	{

		Debug.Log("Sheep Interacted");
	}

	private IEnumerator PetSheep() {
		// Move bee to sheep
		yield return MoveBeeToSheep();
		// play bee petting animation
		// play sheep love sound
		// play sheep love particle
	}

	private IEnumerator MoveBeeToSheep() {
		yield return _beeMovement.MoveUntilObjectReached(transform.position, 2f);
	}

	public void InterruptEvent()
	{
		OnInterruptedDone?.Invoke(this);
	}

	public void StopEvent()
	{
	}

	// Start is called before the first frame update
	void Start()
    {
        CanInterrupt = true;
		MultipleInteractions = false;
    }
}