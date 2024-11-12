using System;
using System.Collections;
using UnityEngine;

[
	RequireComponent(typeof(ToggleRotate)),
	RequireComponent(typeof(PlayParticle)),
]
public class ClambInteraction : MonoBehaviour, 
								IInteractable, 
								IInterruptible, 
								IEvent
{
	public bool CanInterrupt { get; set; }
	public EventState State { get ; set; }
	public bool MultipleInteractions { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

	internal ToggleRotate _toggleRotate;
	internal PlayParticle _playParticle;

	public event Action<IInterruptible> OnInterruptedDone;
	public event Action OnEventDone;

	private void Awake() {
		_toggleRotate = GetComponent<ToggleRotate>();
		_playParticle = GetComponent<PlayParticle>();
	}

	private void Start()
	{
		Setup();
	}

	private void Setup()
	{
		CanInterrupt = true;
		SubscribeToEvents();
	}

	private void SubscribeToEvents()
	{
		_toggleRotate.OnToggleDone += HandleToggleDone;
	}

	public void Interact()
	{
		_toggleRotate.Toggle();
		_playParticle.Toggle();	
	}

	public void InterruptEvent()
	{
		StartCoroutine(Interrupt());
	}

	private IEnumerator Interrupt()
	{
		Debug.Log("Event Interrupted");
		yield return new WaitForSeconds(5f);
		Debug.Log("Event Interrupted Done");
		OnInterruptedDone?.Invoke(this);
	}

	private void HandleToggleDone() {
		OnEventDone?.Invoke();
	}

	private void OnDestroy()
	{
		UnsubscribeFromEvents();
	}

	private void UnsubscribeFromEvents()
	{
		_toggleRotate.OnToggleDone -= HandleToggleDone;
	}
}
