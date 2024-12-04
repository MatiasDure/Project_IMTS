using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(PlayAnimation))]
public class RiverFish : MonoBehaviour
{
	[SerializeField] float _moveSpeed;

	private PlayAnimation _playAnimation;

	private bool _inAnimationSequence = false;

	private const string ANIMATION_JUMP_PARAMETER = "Jump";
	private const string ANIMATION_JUMP_STATE = "Jumping";
	public event Action<RiverFish> OnAnimationFinished;
	
	private void Awake()
	{
		_playAnimation = GetComponent<PlayAnimation>();
	}
	
	private void Start()
	{
		PlayAnimation();
	}

	private void Update()
	{
		if (!_inAnimationSequence) return;
		
		CheckAnimation();
		Move();
	}

	private void Move()
	{
		transform.position += transform.forward * _moveSpeed * Time.deltaTime;
	}

	private void CheckAnimation()
	{
		if (!_playAnimation.IsAnimationOver()) return;
		HandleAnimationDone();
	}
	
	private void HandleAnimationDone()
	{
		_inAnimationSequence = false;
		OnAnimationFinished?.Invoke(this); 
	}

	private void SetTransform(Vector3 position, Quaternion rotation)
	{
		transform.position = position;
		transform.rotation = rotation;
	}

	private void PlayAnimation()
	{
		_playAnimation.SetTrigger(ANIMATION_JUMP_PARAMETER);
		_inAnimationSequence = true;
	}

	public void ResetFish(Vector3 position, Quaternion rotation)
	{
		SetTransform(position, rotation);
		PlayAnimation();
	}
}