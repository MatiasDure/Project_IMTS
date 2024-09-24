using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class InventoryController : MonoBehaviour
{
    [SerializeField] private CollectableContainer _collectableContainerPrefab;
    [SerializeField] private Transform _contentContainer;
    [SerializeField] private GameObject _inventoryUI;
    [SerializeField] private Image _inventoryToggleImage;
    [SerializeField] private Button _inventoryToggleButton;
    [SerializeField] private Sprite _inventoryOpenSprite;
    [SerializeField] private Sprite _inventoryCloseSprite;
    [SerializeField] private AudioClip _openInventory;
    [SerializeField] private AudioClip _closeInventory;

    private List<CollectableContainer> _collectableContainers = new List<CollectableContainer>();
    private bool _isInventoryOpen = false;
    private AudioSource _audioSource;

    private void Awake()
    {
        Inventory.OnCreatureAdded += AddCollectable;
        _inventoryToggleButton.onClick.AddListener(ToggleInventory);
        _audioSource = GetComponent<AudioSource>();
    }

    private void AddCollectable(CreatureInfo creature)
    {
        CollectableContainer collectableContainer = Instantiate(_collectableContainerPrefab, _contentContainer);
        collectableContainer.SetImage(creature.CreatureImage);
        collectableContainer.SetName(creature.CreatureName);
		collectableContainer.SetDialogText(creature.CreatureDialogText);
        _collectableContainers.Add(collectableContainer);
    }

    private void ToggleInventory()
    {
        _isInventoryOpen = !_isInventoryOpen;
        _inventoryUI.SetActive(_isInventoryOpen);
        _inventoryToggleImage.transform.Rotate(0, 0, 180);

        if(_isInventoryOpen )
        {
            _audioSource.clip = _openInventory;
            _audioSource.Play();
            _inventoryToggleImage.sprite = _inventoryCloseSprite;
        }
        else
        {
            _audioSource.clip = _closeInventory;
            _audioSource.Play();
            _inventoryToggleImage.sprite = _inventoryOpenSprite;
        }
    }

    private void OnDestroy()
    {
        Inventory.OnCreatureAdded -= AddCollectable;
    }
}
