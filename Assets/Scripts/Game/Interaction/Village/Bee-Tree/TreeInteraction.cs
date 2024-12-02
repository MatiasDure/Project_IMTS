using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[
    RequireComponent(typeof(PlayAnimation)),
    RequireComponent(typeof(BoxCollider)),
]
public class TreeInteraction : MonoBehaviour, IInteractable, IEvent, IInterruptible
{
    [SerializeField] private ObjectMovement _beeMovement;
    
    [SerializeField] private string _animationTriggerVariable;
    [SerializeField] private string _animationState;
    [SerializeField] private float _resetTime;

    [SerializeField] private Collider _randomRangeCollider;
    [SerializeField] private float _minDistanceFromCenter;
    
    [SerializeField] private Transform _coneHolder;

    [SerializeField] private Transform _inspectPosition;
    
    private int _lastSelectedConeIndex;
    private List<ACone> _cones = new List<ACone>();
    private List<ACone> _conesOnGround = new List<ACone>();
    public bool CanInterrupt { get; set; }
    public bool MultipleInteractions { get; set; }
    
    public event Action OnEventDone;
    public event Action<IInterruptible> OnInterruptedDone;
    public EventState State { get; set; }
    
    internal PlayAnimation _playAnimation;
    internal bool _ready;
    

    private void Awake()
    {
        _playAnimation = GetComponent<PlayAnimation>();
    }
    
    private void Start()
    {
        SetUp();
    }
    
    private void SetUp()
    {
        _ready = true;
        CanInterrupt = true;
        MultipleInteractions = false;
        GetCone();
    }

    private void GetCone()
    {
        foreach (Transform cone in _coneHolder)
        {
            if (cone.GetComponent<ACone>())
            {
                _cones.Add(cone.GetComponent<ACone>());
            }
        }
    }
    
    public void Interact()
    {
        if (_ready)
        {
            StartCoroutine(ShakeTree(GetRandomCone(_cones)));
            
            BeeMoveToInspectPosition();
        }
        
    }

    private IEnumerator ShakeTree(ACone aCone)
    {
        _ready = false;
        
        yield return RepareConeAndPlayAnimation(aCone);

        yield return DropTheCone(aCone);

        OnEventDone?.Invoke();
        _ready = true;
        
        yield return FadeAndResetCone(aCone);

        if (_conesOnGround.Count < 1)
        {
            if(Bee.Instance.State == BeeState.InspectTree) ReleaseBee();
        }
    }

    private IEnumerator FadeAndResetCone(ACone aCone)
    {
        yield return aCone.FadeCone(_resetTime / 2);
        aCone.ResetCone(GetRandomPointInBox());
        _conesOnGround.Remove(aCone);
    }

    private IEnumerator DropTheCone(ACone aCone)
    {
        aCone.DropCone();

        yield return new WaitForSeconds(_resetTime / 2);
    }

    private IEnumerator RepareConeAndPlayAnimation(ACone aCone)
    {
        _playAnimation.SetTrigger(_animationTriggerVariable);

        _conesOnGround.Add(aCone);
        aCone.ResetCone(GetRandomPointInBox());

        yield return WaitForAnimationStateToPlay(_animationState);
    }

    private void BeeMoveToInspectPosition()
    {
        Bee.Instance.UpdateState(BeeState.InspectTree);
        StartCoroutine(MoveBee());
    }

    private IEnumerator MoveBee()
    {
        var targetPosition = _inspectPosition.position;
        while (!_beeMovement.IsInPlace(targetPosition))
        {
            _beeMovement.MoveTo(targetPosition, 3);
            _beeMovement.SnapRotationTowards(targetPosition);
            yield return null;
        }
        _beeMovement.SnapRotationTowards(transform.position);
    }

    private void ReleaseBee()
    {
        Bee.Instance.UpdateState(BeeState.Idle);
    }
    
    private IEnumerator WaitForAnimationStateToPlay(string state)
    {
        yield return StartCoroutine(_playAnimation.WaitForAnimationToStart(state));
        yield return StartCoroutine(_playAnimation.WaitForAnimationToEnd());
    }

    public Vector3 GetRandomPointInBox()
    {
        var colliderBound = _randomRangeCollider.bounds;
        Vector3 center = colliderBound.center;
        Bounds bounds = colliderBound;

        Vector3 randomPosition;

        do
        {
            // Generate random positions within the bounds
            float randomX = Random.Range(bounds.min.x, bounds.max.x);
            float randomY = Random.Range(bounds.min.y, bounds.max.y);
            float randomZ = Random.Range(bounds.min.z, bounds.max.z);

            randomPosition = new Vector3(randomX, randomY, randomZ);
        } 
        while (Vector3.Distance(center, randomPosition) < _minDistanceFromCenter);

        return randomPosition;
    }

    private ACone GetRandomCone(List<ACone> list)
    {
        if (list == null || list.Count == 0)
        {
            return null;
        }
        
        ACone chosen;
        int randomIndex;
        
        do
        {
            randomIndex = Random.Range(0, _cones.Count-1);

            chosen = _cones[randomIndex];
        } 
        while (_conesOnGround.Contains(chosen));
        
        return chosen;
    }
    
    public void InterruptEvent()
    {
        StopCoroutine(MoveBee());
        ReleaseBee();
        
        OnInterruptedDone?.Invoke(this);
    }
    
    public void StopEvent()
    {
        StopAllCoroutines();
    }
}
