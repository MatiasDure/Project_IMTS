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

    [SerializeField] private ObjectMovement _beeMovement;

    public bool CanInterrupt { get; set; }
    public EventState State { get; set; }
    public bool MultipleInteractions { get; set; }

    public event Action OnEventDone;
    public event Action<IInterruptible> OnInterruptedDone;

    private FishSpeedUpBehaviour _speedUpBehaviour;

    private bool _canSpeedUp = true;
    private bool _beeIsChasing = false;

    public void Interact()
    {
        // Fish is already interacted with
        if (!_canSpeedUp) return;

        _canSpeedUp = false;
        _beeIsChasing = true;

        _speedUpBehaviour.BeginEffectSequence();
        StartCoroutine(MoveBeeTowardsFish());
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

    private void StopBeeChasing()
    {
        if (!_beeIsChasing) return;

        _beeIsChasing = false;
        Bee.Instance.UpdateState(BeeState.Idle);
    }

    private IEnumerator MoveBeeTowardsFish()
    {
        Bee.Instance.UpdateState(BeeState.ChasingFish);
        yield return StartCoroutine(MoveBeeToPosition(transform.position));
        StopBeeChasing(); // Reached fish
    }

    private IEnumerator MoveBeeToPosition(Vector3 position)
    {
        while (_beeIsChasing && !_beeMovement.IsInPlace(position, 1))
        {
            _beeMovement.MoveTo(transform.position, 3);
            yield return null;
        }
    }

    private void HandleInteractionDone()
    {
        StopAllCoroutines();
        StopBeeChasing();
        _canSpeedUp = true;


        OnEventDone?.Invoke();
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

    public void InterruptEvent()
    {
        StopAllCoroutines();
        StopBeeChasing();

        OnInterruptedDone?.Invoke(this);
    }
}