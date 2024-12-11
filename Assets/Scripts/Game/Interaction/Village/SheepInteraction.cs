using System;
using System.Collections;
using UnityEngine;

[
	RequireComponent(typeof(BoxCollider)),
	RequireComponent(typeof(SoundComponent)),
	RequireComponent(typeof(PlayParticle))
]
public class SheepInteraction : MonoBehaviour, IInteractable, IEvent, IInterruptible
{
	[SerializeField] private ObjectMovement _beeMovement;
	[SerializeField] private Vector3 _beePettingPositionOffset;
	
	[Header("Sound")]
	[SerializeField] Sound _sheepLoveSound;
	
	[Header("Animation")]
	[SerializeField] private PlayAnimation _beeAnimation;
	[SerializeField] private string _pettingAnimationStateName;
	[SerializeField] private string _pettingAnimationParameterName;
	
	private SoundComponent _soundComponent;
	private Coroutine _petSheepCoroutine;
	private PlayParticle _playParticle;

	public bool CanInterrupt { get; set; }
	public bool MultipleInteractions { get; set; }
	public EventState State { get; set; }

	public event Action<IInterruptible> OnInterruptedDone;
	public event Action OnEventDone;

	private void Awake()
	{
		if(_beeAnimation == null) Debug.LogError("Bee Animator is required by Sheep Interaction to work correctly");
		
		_soundComponent = GetComponent<SoundComponent>();
		_playParticle = GetComponent<PlayParticle>();
	}

	void Start()
    {
        CanInterrupt = true;
		MultipleInteractions = false;
    }

	public void Interact()
	{
		if(_petSheepCoroutine != null) return;

		_petSheepCoroutine = StartCoroutine(PetSheep());
	}

	private IEnumerator PetSheep() {
		Bee.Instance.UpdateState(BeeState.PettingSheep);

		yield return MoveBeeToSheep();
		// This needs to be updated when the petting animation is added, because currently it just disables the swimming animation in the ocean plot
		// yield return PetSheepAnimation();
		yield return SheepReaction();

		Bee.Instance.UpdateState(BeeState.Idle);
		_petSheepCoroutine = null;
		OnEventDone?.Invoke();
	}

	private IEnumerator SheepReaction() {
		_playParticle.ToggleOn();
		_soundComponent.PlaySound(_sheepLoveSound);
		yield return new WaitForSeconds(_sheepLoveSound.clip.length);
	}

	private IEnumerator MoveBeeToSheep() {
		yield return _beeMovement.MoveUntilObjectReached(transform.position + _beePettingPositionOffset, .75f);
		yield return _beeMovement.RotateUntilLookAt(transform.position, .1f);
	}

	private IEnumerator PetSheepAnimation() {
		_beeAnimation.SetBoolParameter(_pettingAnimationParameterName, false);
		yield return _beeAnimation.WaitForAnimationToStart(_pettingAnimationStateName);
		yield return _beeAnimation.WaitForAnimationToEnd();
		_beeAnimation.SetBoolParameter(_pettingAnimationParameterName, true);
	}

	public void InterruptEvent()
	{
		StopPettingSheep();
		Bee.Instance.UpdateState(BeeState.Idle);
		OnInterruptedDone?.Invoke(this);
	}

	public void StopEvent()
	{
		StopPettingSheep();
	}

	private void StopPettingSheep() {
		if(_petSheepCoroutine != null) {
			StopCoroutine(_petSheepCoroutine);
			_petSheepCoroutine = null;
		}
	}
}