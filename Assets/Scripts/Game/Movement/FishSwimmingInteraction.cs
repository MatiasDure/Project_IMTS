using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSwimmingInteraction : MonoBehaviour, IInteractable
{
    [SerializeField] internal float _moveSpeed = 0.1f;
    [SerializeField] private float _speedUpMultiplier = 2f;
    [SerializeField] private float _speedUpDuration = 1.5f;
    [SerializeField] private float _speedChangeDuration = 0.5f;
    [SerializeField] private ParticleSystem _particleSystem;

    private bool _canSpeedUp = true;
    private float _multipliedSpeed;

	public bool CanInterrupt { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
	public bool MultipleInteractions { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

	private void Start()
    {
        _multipliedSpeed = _moveSpeed * _speedUpMultiplier;
    }

    private void FixedUpdate()
    {
        Swim();
    }

    public void Interact()
    {
        if (!_canSpeedUp) return;

        StartCoroutine(FishTappedCoroutine(_moveSpeed, _speedUpDuration));
    }

    private void Swim()
    {
        transform.position += transform.forward * _moveSpeed;
    }

    private void ApplySpeedUpEffect(float originalMoveSpeed, float targetSpeed)
    {
        _canSpeedUp = false;

        StartCoroutine(SmoothSpeedChangeCoroutine(originalMoveSpeed, targetSpeed, _speedChangeDuration));
        _particleSystem.Play();
    }

    private void RemoveSpeedUpEffect(float originalMoveSpeed)
    {
        _canSpeedUp = true;

        StartCoroutine(SmoothSpeedChangeCoroutine(_moveSpeed, originalMoveSpeed, _speedChangeDuration));
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
