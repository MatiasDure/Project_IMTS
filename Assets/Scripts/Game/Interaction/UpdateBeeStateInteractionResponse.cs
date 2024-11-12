using System;
using UnityEngine;

public abstract class UpdateBeeStateInteractionResponse : MonoBehaviour
{
	[SerializeField] protected UpdateActiveEventCollection _updateStateDetails;
	public static event Action<UpdateActiveEventCollection> OnUpdateBeeState;

	protected virtual void UpdateBeeState()
	{
		OnUpdateBeeState?.Invoke(_updateStateDetails);
	}
}
