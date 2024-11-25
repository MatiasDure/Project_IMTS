using System;
using UnityEngine;

public interface IInspectable
{
	public GameObject InspectablePoint { get; }
    public void Inspect();
	public event Action OnInspected;
}
