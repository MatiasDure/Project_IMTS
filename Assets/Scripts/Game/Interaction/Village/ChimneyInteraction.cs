using System;
using System.Collections;
using UnityEngine;

[
	RequireComponent(typeof(PlayParticle)),
	RequireComponent(typeof(PlayAnimation)),
	RequireComponent(typeof(BoxCollider)),
]
public class ChimneyInteraction : MonoBehaviour, IInteractable, IEvent
{
	[SerializeField] private string _smokeAnimationParameterName;
	[SerializeField] private string _shrinkAnimationName = "Shrink";
	[SerializeField] private string _expandAnimationName = "Expand";
	[SerializeField] private float _particleDuration = 3f;

	private PlayParticle _playParticle;
	private PlayAnimation _playAnimation;
	private bool _isPlaying;

	public event Action OnEventDone;

	public bool CanInterrupt { get; set; }
	public bool MultipleInteractions { get; set; }
	public EventState State { get; set; }

	private void Awake()
	{
		_playParticle = GetComponent<PlayParticle>();
		_playAnimation = GetComponent<PlayAnimation>();
	}
	private void Start()
	{
		CanInterrupt = false;
		MultipleInteractions = false;
	}

	public void Interact()
	{
		if(_isPlaying) return;
		StartCoroutine(PlayChimneyAnimation());
	}

	private IEnumerator ShrinkChimney()
	{
		_playAnimation.SetBoolParameter(_smokeAnimationParameterName, true);
		yield return StartCoroutine(_playAnimation.WaitForAnimationToStart(_shrinkAnimationName));
		yield return StartCoroutine(_playAnimation.WaitForAnimationToEnd());
	}

	private IEnumerator ExpandChimney()
	{
		yield return StartCoroutine(_playAnimation.WaitForAnimationToStart(_expandAnimationName));
		yield return StartCoroutine(_playAnimation.WaitForAnimationToEnd());
	}

	private IEnumerator PlayChimneyAnimation()
	{
		_isPlaying = true;
		yield return StartCoroutine(ShrinkChimney());
		_playParticle.ToggleOn();
		yield return StartCoroutine(ExpandChimney());
		_playAnimation.SetBoolParameter(_smokeAnimationParameterName, false);
		yield return new WaitForSeconds(_particleDuration);
		_playParticle.ToggleOff();
		OnEventDone?.Invoke();
		_isPlaying = false;
	}
}
