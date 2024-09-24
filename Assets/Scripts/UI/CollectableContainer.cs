using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CollectableContainer : MonoBehaviour
{
    [SerializeField] private Image _imageComponent;
    [SerializeField] private TextMeshProUGUI  _textComponent;

	private string _dialogText;

	public string DialogText => _dialogText;

    public void SetImage(Sprite sprite)
    {
        _imageComponent.sprite = sprite;
    }

    public void SetName(string text)
    {
        _textComponent.text = text;
    }

	public void SetDialogText(string text) {
		_dialogText = text;
	}
}
