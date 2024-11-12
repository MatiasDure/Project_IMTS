using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour, IInteractable
{
	public bool CanInterrupt { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

	public void Interact()
	{
		transform.position += Vector3.up * 0.2f;
	}
}
