using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CollectableContainer : MonoBehaviour
{
    [SerializeField] private Image _imageComponent;
	[SerializeField] private GameObject _placeholderObject;
	[SerializeField] private GameObject _collectableContainer;
    // [SerializeField] private TextMeshProUGUI  _textComponent;

	private bool _hasCollectable;
	private string _dialogText;
	private Sprite _sprite;

	public bool HasCollectable => _hasCollectable;
	public Sprite Sprite => _sprite;
	public string DialogText => _dialogText;

    public void SetImage(Sprite sprite)
    {
		_imageComponent.gameObject.SetActive(true);
        _imageComponent.sprite = sprite;
		_sprite = sprite;
    }

    // public void SetName(string text)
    // {
    //     _textComponent.text = text;
    // }

	public void SetDialogText(string text) {
		_dialogText = text;
	}

	public void EnableCollectableContainer() {
		_collectableContainer.SetActive(true);
		_hasCollectable = true;
	}

	public void DisablePlaceholder() {
		_placeholderObject.SetActive(false);
	}
}
