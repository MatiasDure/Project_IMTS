using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogManager : MonoBehaviour
{
	public static DialogManager Instance { get; private set; }

	[SerializeField] private GameObject _dialogContainer;
	[SerializeField] private TextMeshProUGUI _dialogText;

	void Awake() {
		if (Instance == null) {
			Instance = this;
		} else {
			Destroy(gameObject);
		}
	}

	public void ShowDialog(string dialog) {
		_dialogText.text = dialog;
		_dialogContainer.SetActive(true);
	}

	public void HideDialog() {
		_dialogContainer.SetActive(false);
	}
}
