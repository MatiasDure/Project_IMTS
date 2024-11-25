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
    [SerializeField] private GameObject _chimney;
	[SerializeField] private string _smokeAnimationParameterName;
	[SerializeField] private string _shrinkAnimationName = "Shrink";
	[SerializeField] private string _expandAnimationName = "Expand";

	private PlayParticle _playParticle;
	private PlayAnimation _playAnimation;

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
		StartCoroutine(PlayChimneyAnimation());
	}

	private IEnumerator ShrinkChimney()
	{
		_playAnimation.SetBoolParameter(_smokeAnimationParameterName, true);
		yield return StartCoroutine(_playAnimation.WaitForAnimationToStart(_shrinkAnimationName));
		Debug.Log("Shrink animation started");
		yield return StartCoroutine(_playAnimation.WaitForAnimationToEnd());
		Debug.Log("Shrink animation ended");
	}

	private IEnumerator ExpandChimney()
	{
		yield return StartCoroutine(_playAnimation.WaitForAnimationToStart(_expandAnimationName));
		yield return StartCoroutine(_playAnimation.WaitForAnimationToEnd());
		Debug.Log("Expand animation ended");
	}

	private IEnumerator PlayChimneyAnimation()
	{
		yield return StartCoroutine(ShrinkChimney());
		_playParticle.ToggleOn();
		yield return StartCoroutine(ExpandChimney());
		_playAnimation.SetBoolParameter(_smokeAnimationParameterName, false);
		yield return new WaitForSeconds(3f);
		_playParticle.ToggleOff();
		OnEventDone?.Invoke();
	}
}
