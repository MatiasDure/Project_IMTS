using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BeeSwimming))]
public class BeeChasingFish : InteractionEvent,
                              IInterruptible
{
    [Tooltip("When the bee gets this close to the target the event is ended prematurely")]
    [SerializeField] float distanceToTargetForceStopInteraction;

    private bool _isChasing = false;
    private bool _initialSmoothRotationFinished = false;
    private BeeSwimming _swimmingScript;
    private Transform _targetTransform;

    public event Action<IInterruptible> OnInterruptedDone;

    private void Awake()
    {
        _swimmingScript = GetComponent<BeeSwimming>();
    }

    void Start()
    {
        SubscribeToEvents();
    }

    void Update()
    {
        if (!_isChasing) return;

        Chase();
        CheckDistanceToTarget();
    }

    private void NewInteractionEventStarted(UpdateInteractionStateCollection eventMetadata)
    {
        if (eventMetadata.State != BeeState.ChasingFish) return;

        _isChasing = true;
        _targetTransform = eventMetadata.Metadata.Target;

        // Smoothly rotate towards the target before following
        StartCoroutine(DoInitialSmoothRotationCoroutine(_swimmingScript.RotationDuration));
    }

    private void BeeStateChanged(BeeState state)
    {
        if (state == BeeState.ChasingFish) return;

        StopChasing();
    }

    private void StopChasing()
    {
        StopAllCoroutines();

        _isChasing = false;
        _targetTransform = null;
    }

    private void Chase()
    {
        if (!_initialSmoothRotationFinished) return;

        transform.rotation =  _swimmingScript.GetRotationToPoint(_targetTransform.position);
    }

    private IEnumerator DoInitialSmoothRotationCoroutine(float duration)
    {
        StartCoroutine(SmoothRotationCoroutine(
            _swimmingScript.GetRotationToPoint(_targetTransform.position), duration));

        yield return new WaitForSeconds(duration);

        _initialSmoothRotationFinished = true;
    }

    private void SubscribeToEvents()
    {
        InteractionEvent.OnInteractionEventStart += NewInteractionEventStarted;
        Bee.OnBeeStateChanged += BeeStateChanged;
    }

    private void UnsubscribeFromEvents()
    {
        InteractionEvent.OnInteractionEventStart -= NewInteractionEventStarted;
        Bee.OnBeeStateChanged -= BeeStateChanged;
    }

    private void OnDestroy()
    {
        UnsubscribeFromEvents();
    }

    IEnumerator SmoothRotationCoroutine(Quaternion targetRotation, float duration)
    {
        Quaternion startRotation = transform.rotation;
        float timeElapsed = 0f;
        float percentageCompleted;

        while (timeElapsed < duration)
        {
            // Slerp from start rotation to the target rotation
            percentageCompleted = timeElapsed / duration;
            SmoothRotate(startRotation, targetRotation, percentageCompleted);
            timeElapsed += Time.deltaTime;

            yield return null;
        }
        // Ensure the final rotation is exactly the target rotation
        transform.rotation = targetRotation;
    }

    private void SmoothRotate(Quaternion startRotation, Quaternion targetRotation, float percentageCompleted)
    {
        transform.rotation = Quaternion.Slerp(startRotation, targetRotation, percentageCompleted);
    }

    private void CheckDistanceToTarget()
    {
        Vector3 currentDistance = transform.position - _targetTransform.position;

        if(currentDistance.magnitude <= distanceToTargetForceStopInteraction)
        {
            ForceStopInteraction();
        }
    }

    private void ForceStopInteraction()
    {
        StopChasing();

        UpdateInteractionStateCollection metadata = GetEndEventMetadata();
        FireInteractEventEnd(metadata);
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

    public void InterruptEvent()
    {
        Debug.Log("WTF2");
        StopChasing();
        OnInterruptedDone?.Invoke(this);
    }
}