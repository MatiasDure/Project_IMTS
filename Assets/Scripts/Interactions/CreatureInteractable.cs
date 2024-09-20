using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CreatureInfo))]
public class CreatureInteractable : Interactable
{
    private CreatureInfo _creature;

    private void Awake()
    {
        _creature = GetComponent<CreatureInfo>();
    }

    public override void OnInteraction()
    {
        base.OnInteraction();
        Debug.Log("I am a creature!");
        Inventory.Instance.AddCreature(_creature);
    }
}
