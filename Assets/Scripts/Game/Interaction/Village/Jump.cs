using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour, IInteractable
{
	public void Interact()
	{
		Debug.Log("Jumping");
	}
}
