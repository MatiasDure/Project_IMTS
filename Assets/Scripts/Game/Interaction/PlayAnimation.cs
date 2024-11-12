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
		Debug.Log($"Setting bool parameter for {parameterName} to {value}");
		Debug.Log($"Bool parameter value is {_animator.GetBool(parameterName)}");
	}

	public bool IsPlaying() => _animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1;

	public bool IsAnimationOver() {
		Debug.Log(_animator.GetCurrentAnimatorStateInfo(0).IsName("InitialChestState"));
		return _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1;
	}

	public bool CurrentAnimationState(string animaitonStateName) => _animator.GetCurrentAnimatorStateInfo(0).IsName(animaitonStateName);
	

	public void Stop() {
		_animator.StopPlayback();
	}
}
