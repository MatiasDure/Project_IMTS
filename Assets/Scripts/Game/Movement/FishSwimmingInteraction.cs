using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSwimmingInteraction : MonoBehaviour, IInteractable
{
    [SerializeField] private float _moveSpeed = 0.1f;
    [SerializeField] private float _speedUpMultiplier = 2f;
    [SerializeField] private float _speedUpDuration = 1.5f;
    [SerializeField] private float _lerpSpeedDuration = 0.5f;
    [SerializeField] private ParticleSystem _ps;

    private bool _canSpeedUp = true;

    public void Interact()
    {
        if (!_canSpeedUp) return;

        StartCoroutine(FishTappedCoroutine(_moveSpeed, _speedUpMultiplier, _speedUpDuration));
    }

    IEnumerator FishTappedCoroutine(float originalMoveSpeed, float speedUpMultiplier, float speedUpDuration)
    {
        float targetSpeed = originalMoveSpeed * speedUpMultiplier;
        ApplyEffect(originalMoveSpeed, targetSpeed);

        _canSpeedUp = false;
        yield return new WaitForSeconds(speedUpDuration);
        _canSpeedUp = true;

        RemoveEffect(originalMoveSpeed);
    }

    IEnumerator SmoothSpeedChangeCoroutine(float originalMoveSpeed, float targetMoveSpeed, float smoothChangeDuration)
    {
        float timeElapsed = 0f;
        
        while(timeElapsed < smoothChangeDuration)
        {
            _moveSpeed = Mathf.Lerp(originalMoveSpeed, targetMoveSpeed, timeElapsed / smoothChangeDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // Ensure move speed reaches the exact target speed
        _moveSpeed = targetMoveSpeed;
    }

    private void FixedUpdate()
    {
        transform.position += transform.forward * _moveSpeed;
    }

    private void ApplyEffect(float originalMoveSpeed, float targetSpeed)
    {
        StartCoroutine(SmoothSpeedChangeCoroutine(originalMoveSpeed, targetSpeed, _lerpSpeedDuration));
        _ps.Play();
    }

    private void RemoveEffect(float originalMoveSpeed)
    {
        StartCoroutine(SmoothSpeedChangeCoroutine(_moveSpeed, originalMoveSpeed, _lerpSpeedDuration));
        _ps.Stop();
    }
}
