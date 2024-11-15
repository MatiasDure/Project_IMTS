using System;
using UnityEngine;

[RequireComponent(typeof(CaughtObject)),]
public class BeeSuckInBurbbleStorm : MonoBehaviour
{
    private CaughtObject _caughtObject;
    private Rigidbody _rigidbody;
    
    public static event Action SuckInStorm;
    
    void Start()
    {
        SetUp();
        SubscribeToEvents();
    }

    private void SetUp()
    {
        _caughtObject = GetComponent<CaughtObject>();
        
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.isKinematic = true;
        _rigidbody.useGravity = false;
    }

    private void HandleStormStart(UpdatePassiveEventCollection eventMetadata) 
    {
        if(eventMetadata.CurrentEvent != PassiveEvent.BurbbleStorm) return;
        _rigidbody.isKinematic = false;
        SuckInStorm?.Invoke();
    }

    private void HandleStormEnd(UpdatePassiveEventCollection eventMetadata)
    {
        if(eventMetadata.PreviousEvent != PassiveEvent.BurbbleStorm) return;

        _rigidbody.isKinematic = true;
    }
    
    private void SubscribeToEvents() {
        PlotEvent.OnPassiveEventStart += HandleStormStart;
        PlotEvent.OnPasiveEventEnd += HandleStormEnd;
    }

    private void UnsubscribeFromEvents() {
        PlotEvent.OnPassiveEventStart -= HandleStormStart;
        PlotEvent.OnPasiveEventEnd -= HandleStormEnd;
    }

    private void OnDestroy() => UnsubscribeFromEvents();
    
}
