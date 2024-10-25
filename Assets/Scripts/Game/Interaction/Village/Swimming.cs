using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swimming : MonoBehaviour, IInteractable
{
	public void Interact()
	{
		transform.position += Vector3.right * 0.2f;
	}
}
