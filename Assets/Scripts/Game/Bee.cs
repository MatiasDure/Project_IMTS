using System;
using UnityEngine;

public class Bee : MonoBehaviour
{
    private BeeState _state;

	public static event Action<BeeState> OnBeeStateChanged;

	private void Start()
	{
		_state = BeeState.FollowingCamera;

		SubscribeToEvents();
	}

	private void SubscribeToEvents()
	{
		HideAndSea.OnHideStart += UpdateState;
		HideAndSea.OnHideEnd += UpdateState;
	}

	private void UnsubscribeFromEvents()
	{
		HideAndSea.OnHideStart -= UpdateState;
		HideAndSea.OnHideEnd -= UpdateState;
	}

	private void UpdateState(UpdateBeeStateCollection newState) {
		_state = newState.State;
		OnBeeStateChanged?.Invoke(_state);
	}

	private void OnDestroy()
	{
		UnsubscribeFromEvents();
	}
}
