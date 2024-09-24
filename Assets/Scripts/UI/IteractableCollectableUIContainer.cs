using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CollectableContainer))]
public class IteractableCollectableUIContainer : Interactable
{
	private CollectableContainer _collectableContainer;
	private void Awake()
	{
		_collectableContainer = GetComponent<CollectableContainer>();
	}
	public override void OnInteraction()
	{
		DialogManager.Instance.ShowDialog(_collectableContainer.DialogText);
	}
}
