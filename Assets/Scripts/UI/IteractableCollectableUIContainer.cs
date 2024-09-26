using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CollectableContainer))]
public class IteractableCollectableUIContainer : Interactable
{
	[SerializeField] private GameObject _newText;
	private CollectableContainer _collectableContainer;
	private void Awake()
	{
		_collectableContainer = GetComponent<CollectableContainer>();
	}
	public override void OnInteraction()
	{
		if(_newText.activeSelf) _newText.SetActive(false);
		
		DialogManager.Instance.ShowDialog(_collectableContainer.DialogText);
	}
}
