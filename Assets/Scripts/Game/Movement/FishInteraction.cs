using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FishSpeedUpBehaviour))]
public class FishInteraction : MonoBehaviour,
                               IInteractable,
                               IInterruptible,
                               IEvent
{
    public bool CanInterrupt { get; set; }
    public EventState State { get; set; }

    public event Action<IInterruptible> OnInterruptedDone;
    public event Action OnEventDone;

    private FishSpeedUpBehaviour _speedUpBehaviour;

    public void Interact()
    {
        if (!_speedUpBehaviour.CanSpeedUp) return;

        _speedUpBehaviour.BeginEffectSequence();
    }

    public void InterruptEvent()
    {
        _speedUpBehaviour.InterruptEffectSequence();
    }

    private void Awake()
    {
        _speedUpBehaviour = GetComponent<FishSpeedUpBehaviour>();
    }

    void Start()
    {
        Setup();
    }

    void Setup()
    {
        CanInterrupt = true;

        SubscribeToEvents();
    }

    private void HandleInteractionDone()
    {
        OnEventDone?.Invoke();
    }

    private void HandleInterruptionDone()
    {
        OnInterruptedDone?.Invoke(this);
    }

    void SubscribeToEvents()
    {
        _speedUpBehaviour.OnEffectDone += HandleInteractionDone;
        _speedUpBehaviour.OnEffectInterrupted += HandleInterruptionDone;
    }

    private void OnDestroy()
    {
        UnsubscribeFromEvents();
    }

    void UnsubscribeFromEvents()
    {
        _speedUpBehaviour.OnEffectDone -= HandleInteractionDone;
        _speedUpBehaviour.OnEffectInterrupted -= HandleInterruptionDone;
    }
}
