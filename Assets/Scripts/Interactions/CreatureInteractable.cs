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
	[SerializeField] private Collider _collider;
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
		_collider = GetComponent<Collider>();
        _interactableSound.OnSoundFinished += CaptureCreature;
    }

    public override void OnInteraction()
    {
		// disable the creature's collider
		_collider.enabled = false;
		OnSpawnableInteracted?.Invoke(_creature.CreatureName);
        StartCoroutine(_interactableSound.PlaySound());
		_isMovingToInventory = true;
		Inventory.Instance.EnableBox();
		// _animator.SetBool("isMoving", true);
    }

	private void Update() {
		if(!_isMovingToInventory)
			transform.LookAt(Camera.main.transform.position + new Vector3(0, -0.3f, 0));

		if (_isMovingToInventory) {
			MoveToInventory();
			if(_animator.speed != 0) {
				_animator.speed = 0;
			}
		}

		if(
			_isDoneMoving && 
			(Inventory.Instance.InventoryBox.transform.position - transform.position).magnitude > 0.1f
		) {
			_isDoneMoving = false;
			_isMovingToInventory = true;
		}

		if(_isDoneMoving && _isSoundDone) {
			Destroy(gameObject);
			Inventory.Instance.DisableBox();
	        Inventory.Instance.AddCreature(_creature);
		}
	}

	private void MoveToInventory() {
		// Get the world position of the UI element
    	// var inventoryPos = InventoryController.Instance.InventoryUI;
		// var test = inventoryPos.TransformPoint(inventoryPos.rect.center) + new Vector3(0, 0, 5);
		// var worldPos = Camera.main.ScreenToWorldPoint(test);

		var inventoryPos = Inventory.Instance.InventoryBox.transform;

		// Move the creature to the inventory slot
		// transform.position = Vector3.MoveTowards(transform.position, worldPos, 4f * Time.deltaTime);
		transform.position = Vector3.MoveTowards(transform.position, inventoryPos.position, 2f * Time.deltaTime);
		transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, 2f * Time.deltaTime);


		if ((inventoryPos.position - transform.position).magnitude <= 0.1f) {
			_isMovingToInventory = false;
			_isDoneMoving = true;
		}
		// if ((worldPos - transform.position).magnitude < 0.1f) {
		// 	_isMovingToInventory = false;
		// 	_isDoneMoving = true;
		// }
	}

    private void CaptureCreature()
    {
		_isSoundDone = true;
        // Destroy(gameObject);
    }

    private void OnDestroy()
    {
        _interactableSound.OnSoundFinished -= CaptureCreature;
    }
}
