using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlotTutorial : MonoBehaviour
{
	[SerializeField] float _lookAroundSkipTime;
	[SerializeField] float _tapSkipTime;
	[Header("")]
	[SerializeField] GameObject _lookPositionsContainer;
	[SerializeField] GameObject _lookTutorialUI;
	[SerializeField] GameObject _tapTutorialObject;
	[SerializeField] GameObject _raycastCamera;
	[SerializeField] RaycastManager _raycastManager;

	private PlotTutorialState _state;
	private List<PlotTutorialLookPosition> _lookPositions;

	private void OnEnable()
	{
		SetupLookPositions();
		UpdateState(PlotTutorialState.LookAround);

		_raycastManager.OnRaycastHit += OnInteract;
	}

	private void OnDisable()
	{
		_raycastManager.OnRaycastHit -= OnInteract;
	}
	
	private void OnInteract(Collider collider)
	{
		if(_state != PlotTutorialState.Tap) return;
		
		UpdateState(PlotTutorialState.Finished);
	}
	
	private IEnumerator SkipTutorialSectionCoroutine(float secondsToWait, PlotTutorialState nextStateToSet)
	{
		yield return new WaitForSeconds(secondsToWait);
		UpdateState(nextStateToSet);
	} 
	
	private void Update()
	{
		CheckState();
	}
	
	private void CheckState()
	{
		switch(_state)
		{
			case PlotTutorialState.LookAround:
				DoRaycast();
				break;
			case PlotTutorialState.Tap:
				break;
		}
	}
	
	private void DoRaycast()
	{
		RaycastHit hit;
		PlotTutorialLookPosition lookPositionScript;

		if (Physics.Raycast(_raycastCamera.transform.position, _raycastCamera.transform.forward, 
			out hit, Mathf.Infinity, Physics.AllLayers, QueryTriggerInteraction.Collide))
			{
				if(hit.collider.TryGetComponent<PlotTutorialLookPosition>(out lookPositionScript))
				{
					DiscoverLookPosition(lookPositionScript);
				}
			}
	}
		
	private void DiscoverLookPosition(PlotTutorialLookPosition lookPosition)
	{
		if (lookPosition.IsDiscovered) return;
		
		lookPosition.DiscoverPosition();
		CheckDiscoveredLookPositions();
	}
	
	private void CheckDiscoveredLookPositions()
	{
		foreach(PlotTutorialLookPosition lookPosition in _lookPositions)
		{
			if (!lookPosition.IsDiscovered) return;
		}
		
		HandleAllPositionsDiscovered();
	}
	
	private void HandleAllPositionsDiscovered()
	{
		_lookPositionsContainer.SetActive(false);
		UpdateState(PlotTutorialState.Tap);
	}
	
	private void UpdateState(PlotTutorialState state)
	{
		_state = state;
		
		switch(_state)
		{
			case PlotTutorialState.LookAround:
				StartLookAroundTutorial();
				break;
			case PlotTutorialState.Tap:
				StartTapTutorial();
				break;
			case PlotTutorialState.Finished:
				FinishTutorial();
				break;
		}
	}
	
	private void StartLookAroundTutorial()
	{
		StopAllCoroutines();
		StartCoroutine(SkipTutorialSectionCoroutine(_lookAroundSkipTime, PlotTutorialState.Tap));

		DisableTutorialObject(_tapTutorialObject);
		EnableTutorialObject(_lookTutorialUI);
	}
	
	private void StartTapTutorial()
	{
		StopAllCoroutines();
		StartCoroutine(SkipTutorialSectionCoroutine(_tapSkipTime, PlotTutorialState.Finished));

		DisableTutorialObject(_lookTutorialUI);
		EnableTutorialObject(_tapTutorialObject);
	}
		
	private void FinishTutorial()
	{
		StopAllCoroutines();
		DisableTutorialObject(_tapTutorialObject);
		gameObject.SetActive(false);
	}
	
	private void EnableTutorialObject(GameObject gameObject)
	{
		gameObject.SetActive(true);
	}
	
	private void DisableTutorialObject(GameObject gameObject)
	{
		gameObject.SetActive(false);
	}
	
	private void SetupLookPositions()
	{
		if (_lookPositionsContainer == null)
		{
			Debug.LogError("Hide spots container was not passed to HideAndSea event");
			return;
		}

		_lookPositions = new List<PlotTutorialLookPosition>();
		foreach(Transform child in _lookPositionsContainer.transform)
		{
			_lookPositions.Add(child.GetComponent<PlotTutorialLookPosition>());
		}
	}
}