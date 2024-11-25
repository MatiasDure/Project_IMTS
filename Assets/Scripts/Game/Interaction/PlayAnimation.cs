using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayAnimation : MonoBehaviour
{
	private Animator _animator;

	void Awake() {
		_animator = GetComponent<Animator>();
	}

	public void Play(string animationName) {
		_animator.Play(animationName);
	}

	public void ToggleBoolParameter(string parameterName) {
		_animator.SetBool(parameterName, !_animator.GetBool(parameterName));
	}

	public void SetBoolParameter(string parameterName, bool value) {
		_animator.SetBool(parameterName, value);
	}

	public bool IsPlaying() => _animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1;

	public bool IsAnimationOver() => _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1;

	public bool CurrentAnimationState(string animaitonStateName) => _animator.GetCurrentAnimatorStateInfo(0).IsName(animaitonStateName);
	
	public IEnumerator WaitForAnimationToStart(string animationName) {
		while(!CurrentAnimationState(animationName)) {
			yield return null;
		}
	}

	public IEnumerator WaitForAnimationToEnd() {
		while(!IsAnimationOver()) {
			Debug.Log("_animation.GetCurrentAnimatorStateInfo(0).normalizedTime: " + _animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
			yield return null;
		}
	}

	public void Stop() {
		_animator.StopPlayback();
	}
}
