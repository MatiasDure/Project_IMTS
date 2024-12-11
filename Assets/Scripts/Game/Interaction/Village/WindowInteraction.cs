using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[
	RequireComponent(typeof(BoxCollider)),
	RequireComponent(typeof(SoundComponent))
]
public class WindowInteraction : MonoBehaviour, IInteractable,
												IInterruptible,
												IEvent
{
	private const string WAVE_ANIMATION_PARAMETER = "IsWaving";
	private const string WAVE_ANIMATION_NAME = "Wave";
	
	[SerializeField] Transform _windowTransform;
	[SerializeField] Transform _windowFrontPosition;
	[SerializeField] ObjectMovement _beeObjectMovement;
	[SerializeField] BeeMovement _beeMovement;
	[SerializeField] PlayAnimation _beePlayAnimation;
	// [SerializeField] PlayAnimation _sheepPlayAnimation;
	
	// Temporary until animations are implemented
	[SerializeField] float _secondsToWaitForAnimations;
	
	public bool CanInterrupt { get; set; } = true;
	public bool MultipleInteractions { get; set; } = false;
	public EventState State { get; set; }
	public event Action<IInterruptible> OnInterruptedDone;
	public event Action OnEventDone;
	
	private WindowInteractionState _interactionState;

	public void Interact()
	{
		Bee.Instance.UpdateState(BeeState.WindowInteraction);
		UpdateState(WindowInteractionState.ApproachingWindow);
	}
	
	private void UpdateState(WindowInteractionState _stateToSet)
	{
		_interactionState = _stateToSet;
		HandleState(_interactionState);
	}
	
	private void HandleState(WindowInteractionState _state)
	{
		switch(_state)
		{
			case WindowInteractionState.ApproachingWindow:
				StartCoroutine(ApproachWindow());
				break;
			case WindowInteractionState.BeeWaving:
				StartCoroutine(Wave());
				break;
			case WindowInteractionState.OpeningWindow:
				StartCoroutine(OpenWindow());
				break;
			case WindowInteractionState.SheepResponds:
				StartCoroutine(SheepResponse());
				break;
			case WindowInteractionState.ClosingWindow:
				StartCoroutine(CloseWindow());
				break;
			case WindowInteractionState.LeavingWindow:
				HandleEventDone();
				break;
		}
	}
	
	// Temporary until animations are implemented
	private IEnumerator DelayCoroutine(float secondsToWait)
	{
		yield return new WaitForSeconds(secondsToWait);
	}
	
	private IEnumerator ApproachWindow()
	{
		yield return StartCoroutine(MoveBeeToPosition(_windowFrontPosition.position));
		UpdateState(WindowInteractionState.BeeWaving);
	}
	
	private IEnumerator Wave()
	{
		_beePlayAnimation.SetBoolParameter(WAVE_ANIMATION_PARAMETER, true);
		yield return _beePlayAnimation.WaitForAnimationToStart(WAVE_ANIMATION_NAME);
		yield return _beePlayAnimation.WaitForAnimationToEnd();
		_beePlayAnimation.SetBoolParameter(WAVE_ANIMATION_PARAMETER, false);
		
		// End sequence prematurely for playtest
		UpdateState(WindowInteractionState.LeavingWindow);
		
		// Normally we would continue with this sequence
		//UpdateState(WindowInteractionState.OpeningWindow);
	}
	
	private IEnumerator OpenWindow()
	{
		Debug.Log("WindowInteraction: Play opening window animation here.");
		yield return StartCoroutine(DelayCoroutine(_secondsToWaitForAnimations));
		UpdateState(WindowInteractionState.SheepResponds);
	}
	
	private IEnumerator SheepResponse()
	{
		Debug.Log("WindowInteraction: Play sheep response animation here.");
		yield return StartCoroutine(DelayCoroutine(_secondsToWaitForAnimations));
		UpdateState(WindowInteractionState.ClosingWindow);
	}
	
	private IEnumerator CloseWindow()
	{
		Debug.Log("WindowInteraction: Play closing window animation here.");
		yield return StartCoroutine(DelayCoroutine(_secondsToWaitForAnimations));
		UpdateState(WindowInteractionState.LeavingWindow);
	}
	
	private void HandleEventDone()
	{
		StopAllCoroutines();
		Bee.Instance.UpdateState(BeeState.Idle);
		OnEventDone?.Invoke();
	}
	
	private IEnumerator MoveBeeToPosition(Vector3 position)
	{
		// Move to the front of the window
		while (!_beeObjectMovement.IsInPlace(position))
		{
			_beeObjectMovement.MoveTo(position, _beeMovement._beeMovementStat.MovementSpeed);
			_beeObjectMovement.SnapRotationTowards(position);
			yield return null;
		}
		
		// Rotate towards the window
		Quaternion targetRotation = 
			Quaternion.LookRotation((_windowTransform.position - _beeMovement.transform.position).normalized);
			yield return StartCoroutine(SmoothRotationCoroutine(targetRotation, 0.25f));
	}
	
	private	IEnumerator SmoothRotationCoroutine(Quaternion targetRotation, float duration)
	{
		Quaternion startRotation = _beeMovement.transform.rotation;
		float timeElapsed = 0f;
		float percentageCompleted;

		while (timeElapsed < duration)
		{
			// Slerp from start rotation to the target rotation
			percentageCompleted = timeElapsed / duration;
			_beeObjectMovement.SmoothRotate(startRotation, targetRotation, percentageCompleted);
			timeElapsed += Time.deltaTime;

			yield return null;
		}

		// Ensure the final rotation is exactly the target rotation
		_beeMovement.transform.rotation = targetRotation;
	}

	public void InterruptEvent()
	{
		HandleEventDone();
		_beePlayAnimation.SetBoolParameter(WAVE_ANIMATION_PARAMETER, false);
		OnInterruptedDone?.Invoke(this);
	}

	public void StopEvent()
	{
		HandleEventDone();
		_beePlayAnimation.SetBoolParameter(WAVE_ANIMATION_PARAMETER, false);
	}
}