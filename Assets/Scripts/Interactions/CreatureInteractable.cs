using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

[RequireComponent(typeof(CreatureInfo)), RequireComponent(typeof(InteractableSound))]
public class CreatureInteractable : Interactable
{
    private CreatureInfo _creature;
    private InteractableSound _interactableSound;

	public static Action<String> OnSpawnableInteracted;

    private void Awake()
    {
        _creature = GetComponent<CreatureInfo>();
        _interactableSound = GetComponent<InteractableSound>();
        _interactableSound.OnSoundFinished += CaptureCreature;
    }

    public override void OnInteraction()
    {
        base.OnInteraction();
		OnSpawnableInteracted?.Invoke(_creature.CreatureName);
        StartCoroutine(_interactableSound.PlaySound());
    }

    private void CaptureCreature()
    {
        Inventory.Instance.AddCreature(_creature);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        _interactableSound.OnSoundFinished -= CaptureCreature;
    }
}
