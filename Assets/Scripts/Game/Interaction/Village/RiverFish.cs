using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(PlayAnimation))]
public class RiverFish : MonoBehaviour
{
	[SerializeField] float _moveSpeed;

	private PlayAnimation _playAnimation;

	private bool _isPlayingAnimation = true;
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
		return;
		CheckAnimation();
		if(!_isPlayingAnimation) return;
		
		if (!_playAnimation.IsPlaying()) return;
		Move();
	}

	private void Move()
	{
		transform.position += transform.forward * _moveSpeed * Time.deltaTime;
	}

	private void CheckAnimation()
	{
		if (!_playAnimation.IsAnimationOver()) return;

		if (!_isPlayingAnimation) return;
		
		_isPlayingAnimation = false;
		Debug.Log("OVER");
		PlayAnimation();
		HandleAnimationDone();
	}
	
	private void HandleAnimationDone()
	{		
		OnAnimationFinished?.Invoke(this); 
	}

	private void SetTransform(Vector3 position, Quaternion rotation)
	{
		transform.position = position;
		transform.rotation = rotation;
	}

	private void PlayAnimation()
	{
		_playAnimation.SetBoolParameter(ANIMATION_JUMP_PARAMETER, true);
		_isPlayingAnimation = true;
	}
		
	public void ResetFish(Vector3 position, Quaternion rotation)
	{
		SetTransform(position, rotation);
		PlayAnimation();
	}
}