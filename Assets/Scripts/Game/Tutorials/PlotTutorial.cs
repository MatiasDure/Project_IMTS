using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlotTutorial : MonoBehaviour
{
	[SerializeField] GameObject _lookPositionsContainer;
	[SerializeField] GameObject _lookTutorialUI;
	[SerializeField] GameObject _tapTutorialUI;
	[SerializeField] GameObject _raycastCamera;
	
	private PlotTutorialState _state;
	private List<PlotTutorialLookPosition> _lookPositions;
	
	private void OnEnable()
	{
		SetupLookPositions();
		UpdateState(PlotTutorialState.LookAround);
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
				//TODO: Implement
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
		DisableTutorialObject(_tapTutorialUI);
		EnableTutorialObject(_lookTutorialUI);
	}
	
	private void StartTapTutorial()
	{
		DisableTutorialObject(_lookTutorialUI);
		EnableTutorialObject(_tapTutorialUI);
	}
	
	private void FinishTutorial()
	{
		//TODO: Implement
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