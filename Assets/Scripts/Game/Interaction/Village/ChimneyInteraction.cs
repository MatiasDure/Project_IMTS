using System;
using System.Collections;
using UnityEngine;

[
	RequireComponent(typeof(SoundComponent)),
	RequireComponent(typeof(PlayParticle)),
	RequireComponent(typeof(PlayAnimation)),
	RequireComponent(typeof(BoxCollider)),
]
public class ChimneyInteraction : MonoBehaviour, IInteractable, IEvent
{
	[SerializeField] private string _houseAnimationParameterName;
	[SerializeField] private string _shrinkAnimationName = "Shrink";
	[SerializeField] private string _expandAnimationName = "Expand";
	[SerializeField] private float _particleDuration = 3f;
	[SerializeField] private Sound _onceTapHouseSFX;
	[SerializeField] private Sound _onceChimneySmokeSFX;

	private PlayParticle _playParticle;
	private PlayAnimation _playAnimation;
	private SoundComponent _soundComponent;
	private bool _isPlaying;

	public event Action OnEventDone;

	public bool CanInterrupt { get; set; }
	public bool MultipleInteractions { get; set; }
	public EventState State { get; set; }

	private void Awake()
	{
		_playParticle = GetComponent<PlayParticle>();
		_playAnimation = GetComponent<PlayAnimation>();
		_soundComponent = GetComponent<SoundComponent>();
	}
	private void Start()
	{
		CanInterrupt = false;
		MultipleInteractions = false;
	}

	public void Interact()
	{
		if(_isPlaying) return;
		
		_soundComponent.PlaySound(_onceTapHouseSFX);
		StartCoroutine(PlayChimneyAnimation());
	}

	private IEnumerator ShrinkHouse()
	{
		_playAnimation.SetBoolParameter(_houseAnimationParameterName, true);
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
		_playParticle.ToggleOn();
		_soundComponent.PlaySound(_onceChimneySmokeSFX);
		yield return StartCoroutine(ShrinkHouse());
		//yield return StartCoroutine(ExpandChimney());
		_playAnimation.SetBoolParameter(_houseAnimationParameterName, false);
		yield return new WaitForSeconds(_particleDuration);
		_playParticle.ToggleOff();
		OnEventDone?.Invoke();
		_isPlaying = false;
	}

	public void StopEvent()
	{
		StopAllCoroutines();
	}
}
