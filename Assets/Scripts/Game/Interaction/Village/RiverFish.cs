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
		//TODO: Implement
	}

	private void CheckAnimation()
	{
		if (!AnimatorIsPlaying())
		{
			HandleAnimationDone();
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
		return _animator.GetCurrentAnimatorStateInfo(0).length >
		   _animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
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