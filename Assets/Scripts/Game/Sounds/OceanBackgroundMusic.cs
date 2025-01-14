using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SoundComponent))]
public class OceanBackgroundMusic : MonoBehaviour
{
	[SerializeField] SoundComponent _musicSoundComponent;
	[SerializeField] Sound _musicSFX;
	
	private void OnEnable()
	{
		_musicSoundComponent.PlaySound(_musicSFX);
	}
	
	private void OnDisable()
	{
		_musicSoundComponent.StopSound();
	}
}
