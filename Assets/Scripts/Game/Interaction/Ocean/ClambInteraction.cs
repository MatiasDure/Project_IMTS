using System;
using System.Collections;
using UnityEngine;

[
	RequireComponent(typeof(PlayAnimation)),
	RequireComponent(typeof(PlayParticle)),
]
public class ClambInteraction : MonoBehaviour, 
								IInteractable, 
								IEvent,
								IToggleComponent
{
	private const string INITIAL_ANIMATION_PARAMETER_NAME = "HasStarted";
	private const string OPEN_ANIMATION_STATE = "clam_open_Animation";
	private const string CLOSE_ANIMATION_STATE = "clam_close_Animation";

	[SerializeField] private string _clamAnimationToggleParameterName;

	public bool CanInterrupt { get; set; }
	public bool MultipleInteractions { get; set; }
	public EventState State { get; set; }
	public ToggleState CurrentToggleState { get; set; }
	public ToggleState NextToggleState { get; set; }

	internal PlayAnimation _playAnimation;
	internal PlayParticle _playParticle;
	internal bool _hasStartedAnimation;

	public event Action OnEventDone;
	public event Action OnToggleDone;

	private void Awake() {
		_playAnimation = GetComponent<PlayAnimation>();
		_playParticle = GetComponent<PlayParticle>();
	}

	private void Start()
	{
		Setup();
	}

	private void Setup()
	{
		CanInterrupt = false;
		MultipleInteractions = false;
		CurrentToggleState = ToggleState.Off;
	}

	public void Interact()
	{
		if(!_hasStartedAnimation) {
			_hasStartedAnimation = true;
			_playAnimation.SetBoolParameter(INITIAL_ANIMATION_PARAMETER_NAME, true);
		}
		
		if(CurrentToggleState == ToggleState.Switching) return;

		Toggle();
	}

	public void ToggleOn()
	{
		StartCoroutine(OpenClam());
	}

	public void ToggleOff()
	{
		StartCoroutine(CloseClam());
	}

	private IEnumerator OpenClam() {
		SetOpenAnimationState();
		yield return StartCoroutine(WaitForAnimationStateToPlay(OPEN_ANIMATION_STATE));
		_playParticle.ToggleOn();
		UpdateState(ToggleState.On);
		OnToggleDone?.Invoke();
	}

	private void SetOpenAnimationState()
	{
		_playAnimation.SetBoolParameter(_clamAnimationToggleParameterName, true);
	}

	private void SetCloseAnimationState()
	{
		_playAnimation.SetBoolParameter(_clamAnimationToggleParameterName, false);
	}

	private IEnumerator CloseClam() {
		_playParticle.ToggleOff();
		SetCloseAnimationState();
		yield return StartCoroutine(WaitForAnimationStateToPlay(CLOSE_ANIMATION_STATE));
		UpdateState(ToggleState.Off);
		OnToggleDone?.Invoke();
	}

	private IEnumerator WaitForAnimationStateToPlay(string state)
	{
		yield return StartCoroutine(_playAnimation.WaitForAnimationToStart(state));
		yield return StartCoroutine(_playAnimation.WaitForAnimationToEnd());
	}

	private void UpdateState(ToggleState state)
	{
		CurrentToggleState = state;
	}

	public void Toggle()
	{
		switch (CurrentToggleState)
		{
			case ToggleState.Off:
				UpdateState(ToggleState.Switching);
				ToggleOn();
				break;
			case ToggleState.On:
				UpdateState(ToggleState.Switching);
				ToggleOff();
				break;
		}
	}
}
