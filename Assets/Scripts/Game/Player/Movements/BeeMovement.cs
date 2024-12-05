using System;
using System.Collections;
using UnityEngine;

[
	RequireComponent(typeof(PlayAnimation)),
	RequireComponent(typeof(AvoidObjectSwimmingBehavior))
]
public class BeeMovement : MonoBehaviour
{
	[Header("Reference")]
    [SerializeField] internal Movement _beeMovementStat;
    [SerializeField] private Transform _otherWorldAnchor;
    [SerializeField] private Transform _portal;
    
    [Header("Setting")]
    [SerializeField] private float _maxRotationAngle = 200f;
    
    [Header("Animation")]
	[Tooltip("The name of the animation parameter that triggers the bee swimming animation")]
	[SerializeField] private string _beeSwimmingAnimationParameterName;
	[Tooltip("The name of the animation state that contains the bee swimming animation")]
	[SerializeField] private string _beeSwimmingAnimationStateName;

	private PlayAnimation _playAnimation;

	private bool _overPortal;
    private Coroutine _portalMovementCoroutine;
	
    private AvoidObjectSwimmingBehavior _avoidObjectSwimmingBehavior;

    public static event Action OnBeeEnteredPlot;

    private void Awake()
    {
	    _playAnimation = GetComponent<PlayAnimation>();
		_avoidObjectSwimmingBehavior = GetComponent<AvoidObjectSwimmingBehavior>();
    }

    private void Start()
    {
	    SubscribeToEvents();
    }

    private void Update()
    {
        Move();
    }

    //TODO: Adjust implementation when handling movement in village plot - see how it goes because
    // the same movement could be used for all 3 plots in my opinion (Orestis). Up to discussion
    private void Move()
    {
		// This is for now until the movement for the village plot is implemented. 
		// Currently the movement is just made for the ocean plot. We should check the Plot and based on that decided the animation, etc.
		if(PlotsManager.Instance._currentPlot != Plot.Ocean) {
			// If the bee is not in the ocean plot, we should stop the swimming animation
			if(_playAnimation.CurrentAnimationState(_beeSwimmingAnimationStateName)) {
				_playAnimation.SetBoolParameter(_beeSwimmingAnimationParameterName, false);
			}
			return;
		}
		if(Bee.Instance.State != BeeState.Idle) return;

		// Playing bee swimming animation if it is not already playing
		if(!_playAnimation.CurrentAnimationState(_beeSwimmingAnimationStateName)) {
			_playAnimation.SetBoolParameter(_beeSwimmingAnimationParameterName, true);
		};

		_avoidObjectSwimmingBehavior.Move(_beeMovementStat.MovementSpeed);
    }

    private IEnumerator MoveThroughPortal()
    {
	    bool isAtPortal = false;
        
		while(!isAtPortal && !_overPortal)
		{
			isAtPortal = MathHelper.AreVectorApproximatelyEqual(transform.position, _portal.position, 0.1f);
			MoveToPortalPosition(_portal.position);
			yield return null;
		}

		EnterPortal(_otherWorldAnchor.position);
		
		OnBeeEnteredPlot?.Invoke();
		Bee.Instance.UpdateState(BeeState.Idle);
		_overPortal = false;
    }

    private void MoveToPortalPosition(Vector3 portalPosition)
    {
		transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(portalPosition - transform.position), _maxRotationAngle * Time.deltaTime);
		transform.position = transform.forward * _beeMovementStat.MovementSpeed * Time.deltaTime + transform.position;
    }
    
    private void EnterPortal(Vector3 targetCameraPosition)
    {
        transform.position = targetCameraPosition + (_portal.position - Camera.main.transform.position);
        _overPortal = true;
    }

    private void HandleGoingToPlot(Plot plot) {
		if(Bee.Instance.State != BeeState.FollowingCamera) return;

		Bee.Instance.UpdateState(BeeState.EnteringPlot);

		if(plot == Plot.Ocean || plot == Plot.Space)
			_portalMovementCoroutine = StartCoroutine(MoveThroughPortal());
	}

	private void SubscribeToEvents()
    {
		ImageTrackingPlotActivatedResponse.OnPlotActivated += HandleGoingToPlot;
		ImageTrackingPlotUpdatedResponse.OnPlotActivated += HandleGoingToPlot;
		ImageTrackingPlotUpdatedResponse.OnPlotDeactivated += HandlePlotDeactivated;
    }

	private void HandlePlotDeactivated(Plot plot)
	{
		if(Bee.Instance.State == BeeState.FollowingCamera) return;

		if(_portalMovementCoroutine != null)
			StopCoroutine(_portalMovementCoroutine);
		
		Bee.Instance.UpdateState(BeeState.FollowingCamera);
		transform.position = Camera.main.transform.position + Camera.main.transform.forward * -2f;
	}

	private void UnSubscribeToEvents()
    {
        ImageTrackingPlotActivatedResponse.OnPlotActivated -= HandleGoingToPlot;
		ImageTrackingPlotUpdatedResponse.OnPlotActivated -= HandleGoingToPlot;
		ImageTrackingPlotUpdatedResponse.OnPlotDeactivated -= HandlePlotDeactivated;
    }

    private void OnDestroy()
    {
        UnSubscribeToEvents();
    }
}

