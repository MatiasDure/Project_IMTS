using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CollectableContainer))]
public class InteractableUIContainerSwitch : Interactable
{
	[SerializeField] private GameObject _newText;
	[SerializeField] private CollectableContainer _collectableContainer;

	public static Action OnCollectableInteracted;

	private void Awake()
	{
		if(_collectableContainer == null) _collectableContainer = GetComponent<CollectableContainer>();
	}

	public override void OnInteraction()
	{
		if(!_collectableContainer.HasCollectable) return;

		if(_newText.activeSelf) {
			OnCollectableInteracted?.Invoke();
			_newText.SetActive(false);
		} 
		
		InventoryController.Instance.OpenInfoSection(_collectableContainer.DialogText, _collectableContainer.Sprite);
	}
}
