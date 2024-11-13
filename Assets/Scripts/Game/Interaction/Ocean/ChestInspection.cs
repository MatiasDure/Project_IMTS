using System;
using UnityEngine;
using System.Collections;

[
	RequireComponent(typeof(PlayAnimation))
]
public class ChestInspection : MonoBehaviour, IInteractable, IEvent, IInterruptible
{
	private const string _startedAnimationParameterName = "HasStarted";
	[SerializeField] private string _boolAnimatorParameterName;
	[SerializeField] private ObjectMovement _beeMovement;
	[SerializeField] private Transform _infrontOfChestPosition;
	[SerializeField] private Transform _inChestPosition;
	[SerializeField] private Movement _beeMovementConfig;
	private PlayAnimation _playAnimation;
	private bool _hasAnimationStarted = false;
	private ChestEventState _chestEventState = ChestEventState.None;

	public event Action OnEventDone;
	public event Action<IInterruptible> OnInterruptedDone;

	public bool CanInterrupt { get; set; }

	public EventState State { get; set; }
	public bool MultipleInteractions { get ; set; }

	private void Awake() {
		_playAnimation = GetComponent<PlayAnimation>();
	}

	private void Start() {
		CanInterrupt = true;
		MultipleInteractions = true;
	}

	public void Interact()
	{
		if(!IsChestInspectionRunning()) {
			Bee.Instance.UpdateState(BeeState.ChestInspection);
			StartCoroutine(InitialChestAnimation());
			return;
		}

		switch (_chestEventState)
		{
			case ChestEventState.InforntOfChest:
				UpdateChestEventState(ChestEventState.GoingInsideChest);				
				break;
			case ChestEventState.InsideChest:
				UpdateChestEventState(ChestEventState.LeavingChest);				
				break;
		}
	}

	public bool IsChestInspectionRunning() => _chestEventState != ChestEventState.None && Bee.Instance.State == BeeState.ChestInspection;

	private IEnumerator InitialChestAnimation() {
		if(!_hasAnimationStarted) {
			_hasAnimationStarted = true;
			EnableChestAnimation();
		}

		SetOpenChestAnimation();

		yield return StartCoroutine(FinishUpdatingAnimationState("OpenChestAnimation"));

		yield return StartCoroutine(FinishPlayingAnimation());

		UpdateChestEventState(ChestEventState.GoingInFrontChest);
	}

	private IEnumerator MoveBeeOutOfChest() {
		SetOpenChestAnimation();
		yield return StartCoroutine(FinishUpdatingAnimationState("OpenChestAnimation"));
		yield return StartCoroutine(FinishPlayingAnimation());
		yield return StartCoroutine(MoveBeeToPosition(_infrontOfChestPosition.position));
		SetCloseChestAnimation();
		yield return StartCoroutine(FinishUpdatingAnimationState("CloseChestAnimation"));
		Bee.Instance.UpdateState(BeeState.FollowingCamera);
		UpdateChestEventState(ChestEventState.None);
		OnEventDone?.Invoke();
	}

	private IEnumerator FinishUpdatingAnimationState(string stateName) {
		while(!_playAnimation.CurrentAnimationState(stateName)) {
			yield return null;
		}
	}

	private void EnableChestAnimation() {
		_playAnimation.SetBoolParameter(_startedAnimationParameterName, true);
	}

	private void SetOpenChestAnimation() {
		_playAnimation.SetBoolParameter(_boolAnimatorParameterName, true);
	}

	private void SetCloseChestAnimation() {
		_playAnimation.SetBoolParameter(_boolAnimatorParameterName, false);
	}

	private IEnumerator FinishPlayingAnimation() {
		while(!_playAnimation.IsAnimationOver()) {
			yield return null;
		}
	}

	private void UpdateChestEventState(ChestEventState state) {
		_chestEventState = state;

		HandleState(state);
	}

	private void HandleState(ChestEventState stateToHandle) {
		switch (stateToHandle)
		{
			case ChestEventState.GoingInFrontChest:
				StartCoroutine(MoveBeeInfrontOfChest());
				break;

			case ChestEventState.GoingInsideChest:
				StartCoroutine(MoveBeeInChest());
				break;

			case ChestEventState.LeavingChest:
				StartCoroutine(MoveBeeOutOfChest());
				break;
		}
	}

	private IEnumerator MoveBeeToPosition(Vector3 position) {
		while(!_beeMovement.IsInPlace(position)) {
			Debug.Log("Moving bee to position");
			_beeMovement.MoveTo(position, _beeMovementConfig.MovementSpeed);
			yield return null;
		}
	}

	private IEnumerator MoveBeeInfrontOfChest() {
		yield return StartCoroutine(MoveBeeToPosition(_infrontOfChestPosition.position));

		_chestEventState = ChestEventState.InforntOfChest;
	}

	private IEnumerator MoveBeeInChest()
	{
		yield return StartCoroutine(MoveBeeToPosition(_inChestPosition.position));
		SetCloseChestAnimation();
		yield return StartCoroutine(FinishUpdatingAnimationState("CloseChestAnimation"));
		yield return StartCoroutine(FinishPlayingAnimation());

		_chestEventState = ChestEventState.InsideChest;
	}

	public void InterruptEvent()
	{
		OnInterruptedDone?.Invoke(this);
	}
}
