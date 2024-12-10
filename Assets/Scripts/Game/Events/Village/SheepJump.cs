using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepJump : PlotEvent
{
    [SerializeField] private Transform _sheepHolder;
    [SerializeField] private float jumpForce;
    internal Cooldown _cooldown = new Cooldown();

    private List<Sheep> _sheeps = new List<Sheep>();

    private void Awake()
    {
        if (_sheepHolder == null)
            Debug.LogError("Missing sheep holder game object reference");
        else
            LoadSheep();
    }
    
    private void Start() {
        SubscribeToEvents();
    }
    
    private void Update() {
        _cooldown.DecreaseCooldown(Time.deltaTime);
    }

    private void LoadSheep()
    {
        foreach (Transform sheep in _sheepHolder)
        {
            if(sheep.GetComponent<Sheep>()) _sheeps.Add(sheep.GetComponent<Sheep>());
        }
    }
    
    public override void StartEvent()
    {
        base.StartEvent();
        Sheep sheep = GetRandomSheep(_sheeps, UnityEngine.Random.Range(0, _sheeps.Count));
        UpdatePassiveEventCollection metadata = SetupStartEventMetadata(sheep.transform);
        
        FireStartEvent(metadata);
        
        sheep.Jump(jumpForce);
        _cooldown.StartCooldown(_config.Timing.Duration);
    }
    
    internal Sheep GetRandomSheep(List<Sheep> sheep, int randomIndex) => sheep[randomIndex];
    
    internal UpdatePassiveEventCollection SetupStartEventMetadata(Transform sheep)
    {
        return new UpdatePassiveEventCollection
        {
            PreviousEvent = PassiveEventManager.Instance.CurrentEventPlaying,
            CurrentEvent = PassiveEvent.SheepJump,
            State = BeeState.Idle,
            Metadata = new EventMetadata
            {
                Target = sheep
            }
        };
    }
    
    internal protected override void HandleWaitingStatus()
    {
        base.HandleWaitingStatus();
        UpdatePassiveEventCollection metadata = SetupEndEventMetadata();

        FireEndEvent(metadata);
    }
    
    protected override void SubscribeToEvents()
    {
        base.SubscribeToEvents();
        _cooldown.OnCooldownOver += UpdateEventStatus;
    }
    
    protected override void UnsubscribeFromEvents()
    {
        base.UnsubscribeFromEvents();
        
        _cooldown.OnCooldownOver -= UpdateEventStatus;
    }
    
    private void OnDestroy()
    {
        UnsubscribeFromEvents();
    }
    
    public override bool CanPlay() => _sheeps.Count > 0;
    
    internal void SetUpPassiveEvent() {
        _state = EventState.InitialWaiting;
    }
    
    protected override void HandlePlotActivated()
    {
        if (PlotsManager.Instance.CurrentPlot != Plot.Village) return;

        SetUpPassiveEvent();
    }
}
