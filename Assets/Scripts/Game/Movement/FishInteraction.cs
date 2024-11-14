using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FishSpeedUpBehaviour))]
public class FishInteraction : InteractionEvent,
                               IInteractable,
                               IEvent
{
    public bool CanInterrupt { get; set; }
    public EventState State { get; set; }

    public event Action OnEventDone;

    private FishSpeedUpBehaviour _speedUpBehaviour;

    public void Interact()
    {
        // Fish is already interacted with
        if (!_speedUpBehaviour.CanSpeedUp) return;

        _speedUpBehaviour.BeginEffectSequence();

        // Bee is already chasing another fish
        if (Bee.Instance.State == BeeState.ChasingFish) return;
        UpdateInteractionStateCollection metadata = GetStartEventMetadata();
        FireInteractEventStart(metadata);
    }

    private void Awake()
    {
        _speedUpBehaviour = GetComponent<FishSpeedUpBehaviour>();
    }

    private void Start()
    {
        Setup();
    }

    private void Setup()
    {
        CanInterrupt = true;

        SubscribeToEvents();
    }

    private void HandleInteractionDone()
    {
        OnEventDone?.Invoke();

        if (Bee.Instance.State != BeeState.ChasingFish) return;

        //TODO: Control the bee here
        UpdateInteractionStateCollection metadata = GetEndEventMetadata();
        FireInteractEventEnd(metadata);
    }

    private void SubscribeToEvents()
    {
        _speedUpBehaviour.OnEffectDone += HandleInteractionDone;
    }

    private void OnDestroy()
    {
        UnsubscribeFromEvents();
    }

    private void UnsubscribeFromEvents()
    {
        _speedUpBehaviour.OnEffectDone -= HandleInteractionDone;
    }

    private UpdateInteractionStateCollection GetStartEventMetadata()
    {
        return new UpdateInteractionStateCollection
        {
            State = BeeState.ChasingFish,
            Metadata = new EventMetadata
            {
                Target = transform
            }
        };
    }

    private UpdateInteractionStateCollection GetEndEventMetadata()
    {
        return new UpdateInteractionStateCollection
        {
            State = BeeState.Idle,
            Metadata = new EventMetadata
            {
                Target = null
            }
        };
    }
}
