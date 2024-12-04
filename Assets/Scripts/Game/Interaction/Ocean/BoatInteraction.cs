using System;
using System.Collections;
using UnityEngine;

[
	RequireComponent(typeof(PlayAnimation)),
	RequireComponent(typeof(BoxCollider)),
	RequireComponent(typeof(AudioSource)),
]
public class BoatInteraction : MonoBehaviour, IInteractable, IEvent
{
	[Tooltip("The name of the animation parameter that will be set to true when the first interaction happens (i.e. HasStarted).")]
	[SerializeField] private string _initialAnimationParameterName;

	[Tooltip("The name of the animation state that will be played when the boat is interacted with.")]
	[SerializeField] private string _boatAnimationState;

	[Tooltip("The name of the animation parameter that will be set to true when the boat is interacted with.")]
	[SerializeField] private string _interactedAnimationParametedName;

	private PlayAnimation _playAnimation;
	private AudioSource _audioSource;
	private bool _hasStartedAnimation;
	private Coroutine _boatInteractionCoroutine;

	public event Action OnEventDone;

	public bool CanInterrupt { get; set; }
	public bool MultipleInteractions { get; set; }
	public EventState State { get; set; }

	void Awake() 
	{
		_playAnimation = GetComponent<PlayAnimation>();
		_audioSource = GetComponent<AudioSource>();
	}

    void Start()
	{
		Setup();
	}

	private void Setup()
	{
		MultipleInteractions = false;
		CanInterrupt = false;
	}

	public void Interact()
	{
		if(_boatInteractionCoroutine != null) return;

		if(!_hasStartedAnimation) {
			_hasStartedAnimation = true;
			_playAnimation.SetBoolParameter(_initialAnimationParameterName, true);
		}

		_boatInteractionCoroutine = StartCoroutine(BoatInteractionCoroutine());
	}

	private IEnumerator BoatInteractionCoroutine()
	{
		_audioSource.Play();
		yield return PlayInteractedAnimation();
		DisableInteractedAnimation();
		_boatInteractionCoroutine = null;
		OnEventDone?.Invoke();
	}

	private IEnumerator PlayInteractedAnimation()
	{
		SetInteractedAnimation();
		yield return _playAnimation.WaitForAnimationToStart(_boatAnimationState);
		yield return _playAnimation.WaitForAnimationToEnd();
	}

	private void SetInteractedAnimation() 
	{
		_playAnimation.SetBoolParameter(_interactedAnimationParametedName, true);
	}

	private void DisableInteractedAnimation() 
	{
		_playAnimation.SetBoolParameter(_interactedAnimationParametedName, false);
	}

	public void StopEvent()	
	{
		if(_boatInteractionCoroutine != null){
			StopCoroutine(_boatInteractionCoroutine);
			DisableInteractedAnimation();
			_boatInteractionCoroutine = null;
		} 
	}
}