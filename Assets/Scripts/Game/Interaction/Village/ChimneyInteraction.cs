using System;
using System.Collections;
using UnityEngine;

[
	RequireComponent(typeof(PlayParticle)),
	RequireComponent(typeof(PlayAnimation)),
	RequireComponent(typeof(BoxCollider)),
]
public class ChimneyInteraction : MonoBehaviour, IInteractable
{
	[Tooltip("The name of the animation parameter that triggers the house animation")]
	[SerializeField] private string _houseAnimationParameterName;
	[Tooltip("The name of the animation state that contains the house animation")]
	[SerializeField] private string _houseAnimationStateName;
	[SerializeField] private float _particleDuration = 3f;

	private PlayParticle _playParticle;
	private PlayAnimation _playAnimation;
	private bool _isPlaying;

	public bool CanInterrupt { get; set; }
	public bool MultipleInteractions { get; set; }

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

	private IEnumerator HouseAnimation()
	{
		_playAnimation.SetBoolParameter(_houseAnimationParameterName, true);
		yield return StartCoroutine(_playAnimation.WaitForAnimationToStart(_houseAnimationStateName));
		yield return StartCoroutine(_playAnimation.WaitForAnimationToEnd());
	}

	private IEnumerator PlayChimneyAnimation()
	{
		_isPlaying = true;
        _playParticle.ToggleOn();
        yield return StartCoroutine(HouseAnimation());
		_playAnimation.SetBoolParameter(_houseAnimationParameterName, false);
		yield return new WaitForSeconds(_particleDuration);
		_playParticle.ToggleOff();
		_isPlaying = false;
	}

	private void OnDisable()
	{
		StopAllCoroutines();
		_playParticle.ToggleOff();
		_isPlaying = false;
	}

}
