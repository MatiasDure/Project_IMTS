using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSwimmingInteraction : MonoBehaviour, IInteractable
{
    [SerializeField] private float _moveSpeed = 0.1f;
    [SerializeField] private float _speedUpMultiplier = 2f;
    [SerializeField] private float _speedUpDuration = 1.5f;
    [SerializeField] private ParticleSystem _ps;

    private bool _canSpeedUp = true;

    public void Interact()
    {
        if (!_canSpeedUp) return;

        StartCoroutine(FishTappedCoroutine(_moveSpeed, _speedUpMultiplier, _speedUpDuration));
    }

    IEnumerator FishTappedCoroutine(float originalMoveSpeed, float speedUpMultiplier, float speedUpDuration)
    {
        ApplyEffect(speedUpMultiplier);

        _canSpeedUp = false;
        yield return new WaitForSeconds(speedUpDuration);
        _canSpeedUp = true;

        RemoveEffect(originalMoveSpeed);
    }

    private void FixedUpdate()
    {
        transform.position += transform.forward * _moveSpeed;
    }

    private void ApplyEffect(float speedUpMultiplier)
    {
        _moveSpeed *= speedUpMultiplier;
        _ps.Play();
    }

    private void RemoveEffect(float originalMoveSpeed)
    {
        _moveSpeed = originalMoveSpeed;
        _ps.Stop();
    }
}
