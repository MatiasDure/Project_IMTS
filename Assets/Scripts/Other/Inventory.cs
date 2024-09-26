using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

	[SerializeField] private GameObject _inventoryBox;
    private List<CreatureInfo> _creatureList = new List<CreatureInfo>();
	private int _objectsMovingTowardsBox = 0;
    public List<CreatureInfo> CreatureList => _creatureList;
	public GameObject InventoryBox => _inventoryBox;
	public int NewObjectsInBox { get; private set;}

    public static Action<CreatureInfo> OnCreatureAdded;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

		InteractableUIContainerSwitch.OnCollectableInteracted += NewObjectInteractedWith;
    }

	public void EnableBox() {
		_objectsMovingTowardsBox++;

		if(_inventoryBox.activeSelf) return;

		_inventoryBox.SetActive(true);
	}

	public void DisableBox() {
		_objectsMovingTowardsBox--;
		
		if(_objectsMovingTowardsBox > 0) return;

		_inventoryBox.SetActive(false);
	}

    public void AddCreature(CreatureInfo creature)
    {
		NewObjectsInBox++;
        _creatureList.Add(creature);
        OnCreatureAdded?.Invoke(creature);
    }

	private void NewObjectInteractedWith() {
		NewObjectsInBox--;
	}

	private void OnDestroy() {
		InteractableUIContainerSwitch.OnCollectableInteracted -= NewObjectInteractedWith;
	}
}
