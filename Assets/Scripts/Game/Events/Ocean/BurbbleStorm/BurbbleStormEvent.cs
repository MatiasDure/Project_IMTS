using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurbbleStormEvent : PlotEvent
{
    [SerializeField] internal GameObject _tornado;
    
    private void Awake()
    {
        if (_tornado == null)
            Debug.LogError("Missing tornado game object reference");
    }

    private void Start() {
        SubscribeToEvents();
        SetUpPassiveEvent();
    }
    private void Update() {
        _cooldown.DecreaseCooldown(Time.deltaTime);
    }
    internal void SetUpPassiveEvent() {
        _state = EventState.InitialWaiting;
        _cooldown.StartCooldown(_config.Timing.StartDelay);
        _frequency.FrequencyAmount = _config.Timing.Frequency;
    }
    
    public override void StartEvent()
    {
        base.StartEvent();
        UpdatePassiveEventCollection metadata = SetupStartEventMetadata(_tornado.transform);
        
        _tornado.SetActive(true);
        FireStartEvent(metadata);
    }

    internal protected override void HandleWaitingStatus()
    {
        if(_state != EventState.Waiting) return;
        
        _tornado.SetActive(false);
        base.HandleWaitingStatus();
        UpdatePassiveEventCollection metadata = SetupEndEventMetadata();

        FireEndEvent(metadata);
    }

    internal protected override void HandleDoneStatus()
    {
        _tornado.SetActive(false);
        base.HandleDoneStatus();
    }

    internal UpdatePassiveEventCollection SetupStartEventMetadata(Transform hideSpot)
    {
        return new UpdatePassiveEventCollection
        {
            PreviousEvent = PassiveEventManager.Instance.CurrentEventPlaying,
            CurrentEvent = PassiveEvent.BurbbleStorm,
            State = BeeState.SuckInStorm,
            Metadata = new EventMetadata
            {
                Target = hideSpot
            }
        };
    }
    
    protected override void SubscribeToEvents()
    {
        base.SubscribeToEvents();
        BeeSuckInBurbbleStorm.SuckInStorm += HandleBeeSuckInStorm;
    }

    protected override void UnsubscribeFromEvents()
    {
        base.UnsubscribeFromEvents();
        BeeSuckInBurbbleStorm.SuckInStorm -= HandleBeeSuckInStorm;
    }
    
    private void HandleBeeSuckInStorm()
    {
        _cooldown.StartCooldown(_config.Timing.Duration);
    }
    
    private void OnDestroy()
    {
        UnsubscribeFromEvents();
    }
}
