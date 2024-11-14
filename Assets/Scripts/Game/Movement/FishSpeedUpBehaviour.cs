using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FishSpeedUpBehaviour : MonoBehaviour
{
    [SerializeField] internal float _moveSpeed = 0.1f;
    [SerializeField] private float _speedUpMultiplier = 2f;
    [SerializeField] private float _speedUpDuration = 1.5f;
    [SerializeField] private float _speedChangeDuration = 0.5f;
    [SerializeField] private ParticleSystem _particleSystem;

    private float _multipliedSpeed;
    private float _originalMoveSpeed;

    public event Action OnEffectDone;

	private void Start()
    {
        _originalMoveSpeed = _moveSpeed;
        _multipliedSpeed = _moveSpeed * _speedUpMultiplier;
    }

    private void FixedUpdate()
    {
        Swim();
    }

    public void BeginEffectSequence()
    {
        StartCoroutine(FishTappedCoroutine(_originalMoveSpeed, _speedUpDuration));
    }

    private void Swim()
    {
        transform.position += transform.forward * _moveSpeed;
    }

    private void ApplySpeedUpEffect(float originalMoveSpeed, float targetSpeed)
    {
        StartCoroutine(SmoothSpeedChangeCoroutine(originalMoveSpeed, targetSpeed, _speedChangeDuration));
        _particleSystem.Play();
    }

    private void RemoveSpeedUpEffect(float originalMoveSpeed)
    {
        StartCoroutine(SmoothSpeedChangeCoroutine(_moveSpeed, originalMoveSpeed, _speedChangeDuration));
        OnEffectDone?.Invoke();
        //TODO: Handle particles from FishInteraction
        _particleSystem.Stop();
    }
    
    private void ChangeMoveSpeed(float originalMoveSpeed, float targetMoveSpeed, float percentageComplete)
    {
        _moveSpeed = Mathf.Lerp(originalMoveSpeed, targetMoveSpeed, percentageComplete);
    }

    IEnumerator FishTappedCoroutine(float originalMoveSpeed, float speedUpDuration)
    {
        ApplySpeedUpEffect(originalMoveSpeed, _multipliedSpeed);

        yield return new WaitForSeconds(speedUpDuration);

        RemoveSpeedUpEffect(originalMoveSpeed);
    }

    internal IEnumerator SmoothSpeedChangeCoroutine(float originalMoveSpeed, float targetMoveSpeed, float smoothChangeDuration)
    {
        float timeElapsed = 0f;
        float percentageComplete;
        
        while(timeElapsed < smoothChangeDuration)
        {
            percentageComplete = timeElapsed / smoothChangeDuration;
            ChangeMoveSpeed(originalMoveSpeed, targetMoveSpeed, percentageComplete);
            timeElapsed += Time.deltaTime;

            yield return null;
        }

        // Ensure move speed reaches the exact target speed
        _moveSpeed = targetMoveSpeed;
    }
}