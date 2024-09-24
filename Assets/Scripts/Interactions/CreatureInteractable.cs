using System;
using System.Collections;
using UnityEngine;

[
	RequireComponent(typeof(CreatureInfo)),
 	RequireComponent(typeof(InteractableSound)), 
	RequireComponent(typeof(Animator))
]
public class CreatureInteractable : Interactable
{
	[SerializeField] private Animator _animator;
    private CreatureInfo _creature;
    private InteractableSound _interactableSound;
	
	private bool _isMovingToInventory = false;
	private bool _isDoneMoving = false;
	private bool _isSoundDone = false;

	public static Action<String> OnSpawnableInteracted;

    private void Awake()
    {
        _creature = GetComponent<CreatureInfo>();
        _interactableSound = GetComponent<InteractableSound>();
		_animator = GetComponent<Animator>();
        _interactableSound.OnSoundFinished += CaptureCreature;
    }

    public override void OnInteraction()
    {
        base.OnInteraction();
		OnSpawnableInteracted?.Invoke(_creature.CreatureName);
        StartCoroutine(_interactableSound.PlaySound());
		_isMovingToInventory = true;
		// _animator.SetBool("isMoving", true);
    }

	private void Update() {
		if (_isMovingToInventory) {
			MoveToInventory();
			if(_animator.speed != 0) {
				_animator.speed = 0;
			}
		}

		if(_isDoneMoving && _isSoundDone) {
			Destroy(gameObject);
		}
	}

	private void MoveToInventory() {
		// Get the world position of the UI element
    	var inventoryPos = InventoryController.Instance.InventoryUI;
		var test = inventoryPos.TransformPoint(inventoryPos.rect.center) + new Vector3(0, 0, 5);

		var worldPos = Camera.main.ScreenToWorldPoint(test);

		// Move the creature to the inventory slot
		transform.position = Vector3.MoveTowards(transform.position, worldPos, 4f * Time.deltaTime);
		transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, 0.7f * Time.deltaTime);

		if ((worldPos - transform.position).magnitude < 0.1f) {
			_isMovingToInventory = false;
			_isDoneMoving = true;
		}
	}

    private void CaptureCreature()
    {
        Inventory.Instance.AddCreature(_creature);
		_isSoundDone = true;
        // Destroy(gameObject);
    }

    private void OnDestroy()
    {
        _interactableSound.OnSoundFinished -= CaptureCreature;
    }
}
