using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour, IInteractable
{
	public void Interact()
	{
		transform.position += Vector3.up * 0.2f;
	}
}
