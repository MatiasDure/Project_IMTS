using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RiverFish : MonoBehaviour
{
	[SerializeField] float _moveSpeed;
	[SerializeField] Animator _animator;

	private bool _inInteractionSequence = false;

	private const string ANIMATION_JUMP_PARAMETER = "Jump";
	private const string ANIMATION_JUMP_STATE = "Jumping";
	public event Action<RiverFish> OnAnimationFinished;
	
	private void Start()
	{
		Initialize();
	}
	
	private void Initialize()
	{
		PlayAnimation();
		_inInteractionSequence = true;
	}

	private void Update()
	{
		if (!_inInteractionSequence) return;
		
		Move();
		CheckAnimation();
	}

	private void Move()
	{
		transform.position += transform.forward * _moveSpeed * Time.deltaTime;
	}

	private void CheckAnimation()
	{
		if (!AnimatorIsPlaying())
		{
			//HandleAnimationDone();
		}
	}
	
	private void HandleAnimationDone()
	{
		_inInteractionSequence = false;
		_animator.SetBool(ANIMATION_JUMP_PARAMETER, false);
		OnAnimationFinished?.Invoke(this);
	}

	private bool AnimatorIsPlaying()
	{
		AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
		Debug.Log(stateInfo.IsName(ANIMATION_JUMP_STATE));
		return stateInfo.IsName(ANIMATION_JUMP_STATE) && 
			   stateInfo.length > stateInfo.normalizedTime;
	}

	private void SetTransform(Vector3 position, Quaternion rotation)
	{
		transform.position = position;
		transform.rotation = rotation;
	}

	private void PlayAnimation()
	{
		_animator.SetBool(ANIMATION_JUMP_PARAMETER, true);
	}
		
	public void ResetFish(Vector3 position, Quaternion rotation)
	{
		SetTransform(position, rotation);
		PlayAnimation();
		
		_inInteractionSequence = true;
	}
}