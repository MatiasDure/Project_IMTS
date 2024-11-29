using System;
using UnityEngine;

[Serializable]
public class Sound
{
	public AudioClip clip;
	[Range(0f, 1f)]
	public float volume = 1f;
	[Range(0, 256)]
	public int priority = 128;
	public bool loop;
}