using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FishSpeedUpBehaviour))]
public class FishInteraction : MonoBehaviour,
							   IInteractable,
							   IInterruptible,
							   IEvent
{


	[SerializeField] private ObjectMovement _beeMovement;

	public bool CanInterrupt { get; set; } = true;
	public EventState State { get; set; }
	public bool MultipleInteractions { get; set; } = false;

	public event Action OnEventDone;
	public event Action<IInterruptible> OnInterruptedDone;

	private FishSpeedUpBehaviour _speedUpBehaviour;

	private bool _canSpeedUp = true;
	private bool _beeIsChasing = false;

	public void Interact()
	{
		// Fish is already interacted with
		if (!_canSpeedUp) return;

		_canSpeedUp = false;
		_beeIsChasing = true;

		_speedUpBehaviour.BeginEffectSequence();
		MoveBeeTowardsFish();
	}

	private void Awake()
	{
		_speedUpBehaviour = GetComponent<FishSpeedUpBehaviour>();
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

	private void StopBeeChasing()
	{
		if (!_beeIsChasing) return;

		_beeIsChasing = false;
		Bee.Instance.UpdateState(BeeState.Idle);
	}

	private void MoveBeeTowardsFish()
	{
		Bee.Instance.UpdateState(BeeState.ChasingFish);
		StartCoroutine(MoveBeeToPosition(transform.position));
	}

	private IEnumerator MoveBeeToPosition(Vector3 position)
	{
		Quaternion targetRotation = 
			Quaternion.LookRotation((transform.position - _beeMovement.transform.position).normalized);
		yield return StartCoroutine(SmoothRotationCoroutine(targetRotation, 0.25f));
		// Snap to the target rotation after smooth rotation

		while (_beeIsChasing && !_beeMovement.IsInPlace(position))
		{
			_beeMovement.MoveTo(transform.position, 3);
			_beeMovement.SnapRotationTowards(transform.position);
			yield return null;
		}

		StopBeeChasing(); // Reached fish
	}

	IEnumerator SmoothRotationCoroutine(Quaternion targetRotation, float duration)
	{
		Quaternion startRotation = _beeMovement.transform.rotation;
		float timeElapsed = 0f;
		float percentageCompleted;

		while (timeElapsed < duration)
		{
			// Slerp from start rotation to the target rotation
			percentageCompleted = timeElapsed / duration;
			_beeMovement.SmoothRotate(startRotation, targetRotation, percentageCompleted);
			timeElapsed += Time.deltaTime;

			yield return null;
		}

		// Ensure the final rotation is exactly the target rotation
		_beeMovement.transform.rotation = targetRotation;
	}

	private void HandleInteractionDone()
	{
		StopAllCoroutines();
		StopBeeChasing();
		_canSpeedUp = true;


		OnEventDone?.Invoke();
	}

	private void SubscribeToEvents()
	{
		_speedUpBehaviour.OnEffectDone += HandleInteractionDone;
	}

	private void OnDestroy()
	{
		UnsubscribeFromEvents();
	}

	private void UnsubscribeFromEvents()
	{
		_speedUpBehaviour.OnEffectDone -= HandleInteractionDone;
	}

	public void InterruptEvent()
	{
		StopAllCoroutines();
		StopBeeChasing();

        OnInterruptedDone?.Invoke(this);
    }

	public void StopEvent()
	{
		StopAllCoroutines();
	}
}