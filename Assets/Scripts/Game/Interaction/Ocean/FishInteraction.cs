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
    [SerializeField] private BeeSwimming _tempMovementSpeedGetter;

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
        if (!_canSpeedUp && _beeIsChasing) return;

        if (!_canSpeedUp && !_beeIsChasing)
        {
            MoveBeeTowardsFish();
            Debug.Log(1);
            return;
        }

        _canSpeedUp = false;
        _beeIsChasing = true;

        _speedUpBehaviour.BeginEffectSequence();
        MoveBeeTowardsFish();
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

    private void MoveBeeTowardsFish()
    {
        Bee.Instance.UpdateState(BeeState.ChasingFish);
        StartCoroutine(MoveBeeToPosition(transform.position));
    }

    private IEnumerator MoveBeeToPosition(Vector3 position)
    {
        while (_beeIsChasing && !_beeMovement.IsInPlace(position))
        {
            _beeMovement.MoveTo(transform.position, 3);
            yield return null;
        }
    }

    private void HandleInteractionDone()
    {
        StopAllCoroutines();
        _canSpeedUp = true;
        _beeIsChasing = false;
        OnEventDone?.Invoke();
        Bee.Instance.UpdateState(BeeState.Idle);
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
        Bee.Instance.UpdateState(BeeState.Idle);
        _beeIsChasing = false;
        OnInterruptedDone?.Invoke(this);
    }
}