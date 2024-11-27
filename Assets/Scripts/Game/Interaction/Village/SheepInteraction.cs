using System;
using UnityEngine;

public class SheepInteraction : MonoBehaviour, IInteractable, IEvent, IInterruptible
{
	[SerializeField] private ObjectMovement _beeMovement;

	public bool CanInterrupt { get; set; }
	public bool MultipleInteractions { get; set; }
	public EventState State { get; set; }

	public event Action<IInterruptible> OnInterruptedDone;
	public event Action OnEventDone;

	public void Interact()
	{
		Debug.Log("Sheep Interacted");
	}

	public void InterruptEvent()
	{
		OnInterruptedDone?.Invoke(this);
	}

	// Start is called before the first frame update
	void Start()
    {
        CanInterrupt = true;
		MultipleInteractions = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}