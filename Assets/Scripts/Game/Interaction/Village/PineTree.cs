using System.Collections;
using UnityEngine;

[
	RequireComponent(typeof(PlayAnimation)),
	RequireComponent(typeof(BoxCollider)),
]
public class PineTree : MonoBehaviour, IInteractable
{
	[Tooltip("The name of the animation parameter that triggers the tree animation")]
	[SerializeField] private string _pineTreeAnimationParameterName;
	
	[Tooltip("The name of the animation state that contains the tree animation")]
	[SerializeField] private string _pineTreeAnimationStateName;

	private PlayAnimation _playAnimation;
	private bool _isPlaying;
	
	public bool CanInterrupt { get; set; }
	public bool MultipleInteractions { get; set; }

	private void Awake()
	{
		_playAnimation = GetComponent<PlayAnimation>();
	}

	void Start()
    {
        CanInterrupt = false;
		MultipleInteractions = false;
    }

	public void Interact()
	{
		if(_isPlaying) return;

		_isPlaying = true;
		StartCoroutine(TreeAnimation());
	}

	private IEnumerator TreeAnimation()
	{
		_playAnimation.SetBoolParameter(_pineTreeAnimationParameterName, true);
		yield return StartCoroutine(_playAnimation.WaitForAnimationToStart(_pineTreeAnimationStateName));
		yield return StartCoroutine(_playAnimation.WaitForAnimationToEnd());
		_playAnimation.SetBoolParameter(_pineTreeAnimationParameterName, false);
		_isPlaying = false;
	}

	private void OnDisable()
	{
		ResetState();
	}

	private void ResetState() 
	{
		_isPlaying = false;
		StopAllCoroutines();
		_playAnimation.SetBoolParameter(_pineTreeAnimationParameterName, false);
	}
}
