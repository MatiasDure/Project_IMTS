using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CollectableContainer : MonoBehaviour
{
    [SerializeField] private Image _imageComponent;
    [SerializeField] private TextMeshProUGUI  _textComponent;

    public void SetImage(Sprite sprite)
    {
        _imageComponent.sprite = sprite;
    }

    public void SetText(string text)
    {
        _textComponent.text = text;
    }
}
