using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private CollectableContainer _collectableContainerPrefab;
    [SerializeField] private Transform _contentContainer;
    [SerializeField] private GameObject _inventoryUI;
    [SerializeField] private Image _inventoryToggleImage;
    [SerializeField] private Button _inventoryToggleButton;
    [SerializeField] private Sprite _inventoryOpenSprite;
    [SerializeField] private Sprite _inventoryCloseSprite;

    private List<CollectableContainer> _collectableContainers = new List<CollectableContainer>();
    private bool _isInventoryOpen = false;

    private void Awake()
    {
        Inventory.OnCreatureAdded += AddCollectable;
        _inventoryToggleButton.onClick.AddListener(ToggleInventory);
    }

    private void AddCollectable(CreatureInfo creature)
    {
        CollectableContainer collectableContainer = Instantiate(_collectableContainerPrefab, _contentContainer);
        collectableContainer.SetImage(creature.CreatureImage);
        collectableContainer.SetText(creature.CreatureName);
        _collectableContainers.Add(collectableContainer);
    }

    private void ToggleInventory()
    {
        _isInventoryOpen = !_isInventoryOpen;
        _inventoryUI.SetActive(_isInventoryOpen);
        _inventoryToggleImage.transform.Rotate(0, 0, 180);
        _inventoryToggleImage.sprite = _isInventoryOpen ? _inventoryCloseSprite : _inventoryOpenSprite;
    }

    private void OnDestroy()
    {
        Inventory.OnCreatureAdded -= AddCollectable;
    }
}
