using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingRodPassive : PlotEvent, IInterruptible
{
	private const string LOOK_DOWN_ANIMATION_PARAMETER = "IsLookingDown";
	[SerializeField] float _lookingAtRiverDuration;
	[SerializeField] Transform _treeStumpHover;
	[Tooltip("Look at this position in front of the tree stump before looking down with animation")]
	[SerializeField] Transform _riverLook;
	[SerializeField] BeeMovement _beeMovement;
	[SerializeField] ObjectMovement _beeObjectMovement;
	[SerializeField] PlayAnimation _beePlayAnimation;
	[SerializeField] RotateObject _fishingRodRotate;

	public event Action<IInterruptible> OnInterruptedDone;
	
	private FishingRodPassiveState _fishingRodEventState;

	public override bool CanPlay() => _fishingRodEventState == FishingRodPassiveState.None;

	public void InterruptEvent()
	{
		LeaveStump();
		OnInterruptedDone?.Invoke(this);
	}
	
	public override void StartEvent()
	{
		base.StartEvent();
		UpdatePassiveEventCollection metadata = SetupStartEventMetadata();

		FireStartEvent(metadata);
		
		Bee.Instance.UpdateState(BeeState.Fishing);
		UpdateState(FishingRodPassiveState.ApproachingStump);
	}
	
	private void UpdateState(FishingRodPassiveState stateToUpdate)
	{
		_fishingRodEventState = stateToUpdate;
		HandleState(_fishingRodEventState);
	}
	
	private void HandleState(FishingRodPassiveState state)
	{
		switch(state)
		{
			case FishingRodPassiveState.ApproachingStump:
				StartCoroutine(ApproachStump());
				break;
			case FishingRodPassiveState.LookingAtRiver:
				StartCoroutine(LookAtRiver());
				break;
			case FishingRodPassiveState.LeavingStump:
				LeaveStump();
				break;
		}
	}
	
	private IEnumerator ApproachStump()
	{
		yield return MoveBeeToPosition(_treeStumpHover.position);
		UpdateState(FishingRodPassiveState.LookingAtRiver);
	}
	
	private IEnumerator LookAtRiver()
	{
		_beePlayAnimation.SetTrigger(LOOK_DOWN_ANIMATION_PARAMETER);
		_fishingRodRotate.EnableRotation();
		
		yield return DelayCoroutine(_lookingAtRiverDuration);
		UpdateState(FishingRodPassiveState.LeavingStump);
	}
	
	private void LeaveStump()
	{
		StopAnimations();
		Bee.Instance.UpdateState(BeeState.Idle);
		HandleEventDone();
	}
	
	private void HandleEventDone()
	{
		StopAnimations();
		_fishingRodEventState = FishingRodPassiveState.None;
		FireEndEvent(SetupEndEventMetadata());
	}
	
	private void StopAnimations()
	{
		StopAllCoroutines();
		_fishingRodRotate.DisableRotation();
	}
	
	// Temporary until fishing rod animation is implemented
	private IEnumerator DelayCoroutine(float secondsToWait)
	{
		yield return new WaitForSeconds(secondsToWait);
	}
	
	private IEnumerator MoveBeeToPosition(Vector3 position)
	{
		while (!_beeObjectMovement.IsInPlace(position))
		{
			_beeObjectMovement.MoveTo(position, _beeMovement._beeMovementStat.MovementSpeed);
			_beeObjectMovement.SnapRotationTowards(position);
			yield return null;
		}

		Quaternion targetRotation = 
			Quaternion.LookRotation((_riverLook.position - _beeMovement.transform.position).normalized);
		yield return StartCoroutine(SmoothRotationCoroutine(targetRotation, 0.25f));
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
			_beeObjectMovement.SmoothRotate(startRotation, targetRotation, percentageCompleted);
			timeElapsed += Time.deltaTime;

			yield return null;
		}

		// Ensure the final rotation is exactly the target rotation
		_beeMovement.transform.rotation = targetRotation;
	}
	
	protected override void HandlePlotActivated()
	{	
		if (PlotsManager.Instance.CurrentPlot != Plot.Village) return;

		SetUpPassiveEvent();
	}
	
	private void SetUpPassiveEvent()
	{
		_state = EventState.InitialWaiting;
	}

	private UpdatePassiveEventCollection SetupStartEventMetadata()
	{
		return new UpdatePassiveEventCollection
		{
			PreviousEvent = PassiveEventManager.Instance.CurrentEventPlaying,
			CurrentEvent = PassiveEvent.FishingRod,
		};
	}
}
