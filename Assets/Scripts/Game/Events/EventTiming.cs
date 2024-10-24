using System;
using UnityEngine;

[Serializable]
public struct EventTiming
{
	[Tooltip("How many times the event gets repeated")]
    public uint Frequency;
	[Tooltip("Time before event can trigger again")]
	public float Cooldown;
	[Tooltip("How long does the event last")]
	public float Duration;
	[Tooltip("Time before event starts")]
	public float StartDelay;
}
