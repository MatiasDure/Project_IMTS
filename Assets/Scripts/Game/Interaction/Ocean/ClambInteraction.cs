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
	private const string OPEN_ANIMATION_STATE = "OpenClam";
	private const string CLOSE_ANIMATION_STATE = "CloseClam";

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
		_playAnimation.SetBoolParameter(_clamAnimationToggleParameterName, true);
		yield return StartCoroutine(_playAnimation.WaitForAnimationToStart(OPEN_ANIMATION_STATE));
		yield return StartCoroutine(_playAnimation.WaitForAnimationToEnd());
		_playParticle.ToggleOn();
		UpdateState(ToggleState.On);	
	}

	private IEnumerator CloseClam() {
		_playParticle.ToggleOff();
		_playAnimation.SetBoolParameter(_clamAnimationToggleParameterName, false);
		yield return StartCoroutine(_playAnimation.WaitForAnimationToStart(CLOSE_ANIMATION_STATE));
		yield return StartCoroutine(_playAnimation.WaitForAnimationToEnd());
		UpdateState(ToggleState.Off);
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
