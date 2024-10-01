using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class InventoryController : MonoBehaviour
{
	public static InventoryController Instance { get; private set; }

    // [SerializeField] private CollectableContainer _collectableContainerPrefab;
    // [SerializeField] private Transform _contentContainer;
    [SerializeField] private GameObject _inventoryUI;
    // [SerializeField] private Image _inventoryToggleImage;
    [SerializeField] private Button _inventoryToggleButton;
    // [SerializeField] private Sprite _inventoryOpenSprite;
    // [SerializeField] private Sprite _inventoryCloseSprite;
    // [SerializeField] private AudioClip _openInventory;
    // [SerializeField] private AudioClip _closeInventory;
	[SerializeField] private AudioClip _uiButtonClicked;
	[SerializeField] private List<CollectableContainer> _collectableContainers;
	[SerializeField] private GameObject _infoSection;
	[SerializeField] private GameObject _objectsCollectedSection;
	[SerializeField] private Image _infoSectionImage;
	[SerializeField] private TextMeshProUGUI _infoSectionText;

    // private List<CollectableContainer> _collectableContainers = new List<CollectableContainer>();
    private bool _isInventoryOpen = false;
    private AudioSource _audioSource;

	private RectTransform _inventoryUIRectTransform;
	public RectTransform InventoryUI => _inventoryUIRectTransform;

    private void Awake()
    {
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			Destroy(gameObject);
		}
		_inventoryUIRectTransform = _inventoryToggleButton.GetComponent<RectTransform>();
        Inventory.OnCreatureAdded += AddCollectable;
        _inventoryToggleButton.onClick.AddListener(ToggleInventory);
        _audioSource = GetComponent<AudioSource>();
    }

	private void Start() {
		_audioSource.clip = _uiButtonClicked;
	}

    // private void AddCollectable(CreatureInfo creature)
    // {
    //     CollectableContainer collectableContainer = Instantiate(_collectableContainerPrefab, _contentContainer);
    //     collectableContainer.SetImage(creature.CreatureImage);
    //     collectableContainer.SetName(creature.CreatureName);
	// 	collectableContainer.SetDialogText(creature.CreatureDialogText);
    //     _collectableContainers.Add(collectableContainer);
    // }

	private void AddCollectable(CreatureInfo creature)
	{
		foreach(var collectable in _collectableContainers) {
			if(collectable.HasCollectable) continue;
				collectable.EnableCollectableContainer();
				collectable.SetImage(creature.CreatureImage);
				// collectable.SetName(creature.CreatureName);
				collectable.SetDialogText(creature.CreatureDialogText);
				collectable.DisablePlaceholder();
				return;
		}
		// CollectableContainer collectableContainer = Instantiate(_collectableContainerPrefab, _contentContainer);
		// collectableContainer.SetImage(creature.CreatureImage);
		// collectableContainer.SetName(creature.CreatureName);
	}
    private void ToggleInventory()
    {
        _isInventoryOpen = !_isInventoryOpen;
        _inventoryUI.SetActive(_isInventoryOpen);
        // _inventoryToggleImage.transform.Rotate(0, 0, 180);

        // if(_isInventoryOpen )
        // {
        //     _audioSource.clip = _openInventory;
        //     // _inventoryToggleImage.sprite = _inventoryCloseSprite;
        // }
        // else
        // {
        //     _audioSource.clip = _closeInventory;
        //     // _inventoryToggleImage.sprite = _inventoryOpenSprite;
        // }
		if(_isInventoryOpen) {
			_inventoryToggleButton.gameObject.SetActive(false);
		}

		_audioSource.clip = _uiButtonClicked;
		_audioSource.Play();
    }

	public void PlayButtonClickedSound() {
		_audioSource.Play();
	}

	public void OpenInfoSection(string infoText, Sprite infoImage) {
		_infoSectionText.text = infoText;
		_infoSectionImage.sprite = infoImage;
		_infoSection.SetActive(true);
		_objectsCollectedSection.SetActive(false);
	}

	public void CloseInfoSection() {
		_infoSection.SetActive(false);
		_objectsCollectedSection.SetActive(true);
	}

	public void CloseInventory() {
		_isInventoryOpen = false;
		_inventoryUI.SetActive(_isInventoryOpen);
		// _audioSource.clip = _closeInventory;
		// _audioSource.Play();
		_audioSource.clip = _uiButtonClicked;
		_audioSource.Play();
		CloseInfoSection();
		_inventoryToggleButton.gameObject.SetActive(true);

		// _inventoryToggleImage.sprite = _inventoryOpenSprite;
	}
    private void OnDestroy()
    {
        Inventory.OnCreatureAdded -= AddCollectable;
    }
}
