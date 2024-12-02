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

    private List<int> _remainingConeIndex = new List<int>();
    private int _lastSelectedConeIndex;
    private List<Cone> _cones = new List<Cone>();
    private List<Cone> _conesOnGround = new List<Cone>();
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
            if (cone.GetComponent<Cone>())
            {
                _cones.Add(cone.GetComponent<Cone>());
            }
        }
    }
    
    public void Interact()
    {
        if (_ready)
        {
            StartCoroutine(ShakeTree(GetRandomConeListWithCycle(_cones)));
            
            BeeMoveToInspectPosition();
        }
        
    }

    private IEnumerator ShakeTree(Cone cone)
    {
        _ready = false;
        
        yield return RepareConeAndPlayAnimation(cone);

        yield return DropTheCone(cone);

        OnEventDone?.Invoke();
        _ready = true;
        
        yield return FadeAndResetCone(cone);

        if (_conesOnGround.Count < 1)
        {
            if(Bee.Instance.State == BeeState.InspectTree) ReleaseBee();
        }
    }

    private IEnumerator FadeAndResetCone(Cone cone)
    {
        yield return cone.FadeCone(_resetTime / 2);
        cone.ResetCone(GetRandomPointInBox());
        _conesOnGround.Remove(cone);
    }

    private IEnumerator DropTheCone(Cone cone)
    {
        cone.DropCone();

        yield return new WaitForSeconds(_resetTime / 2);
    }

    private IEnumerator RepareConeAndPlayAnimation(Cone cone)
    {
        _playAnimation.SetTrigger(_animationTriggerVariable);

        _conesOnGround.Add(cone);
        cone.ResetCone(GetRandomPointInBox());

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
    
    public TCone GetRandomConeListWithCycle<TCone>(List<TCone> list)
    {
        if (list == null || list.Count == 0)
        {
            return default;
        }
        
        if (_remainingConeIndex.Count == 0)
        {
            for (int i = 0; i < list.Count; i++)
            {
                _remainingConeIndex.Add(i);
            }
            Shuffle(_remainingConeIndex);
        }
        
        int selectedIndex = _remainingConeIndex[0];
        _remainingConeIndex.RemoveAt(0);
        
        if (selectedIndex == _lastSelectedConeIndex && list.Count > 1)
        {
            _remainingConeIndex.Add(selectedIndex); 
            selectedIndex = _remainingConeIndex[0];
            _remainingConeIndex.RemoveAt(0);
        }

        _lastSelectedConeIndex = selectedIndex;
        return list[selectedIndex];
    }
    
    private void Shuffle(List<int> indices)
    {
        for (int i = indices.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (indices[i], indices[j]) = (indices[j], indices[i]);
        }
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
