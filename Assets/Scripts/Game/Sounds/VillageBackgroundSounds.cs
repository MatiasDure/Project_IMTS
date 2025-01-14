using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageBackgroundSounds : MonoBehaviour
{
	[SerializeField] SoundComponent _riverSoundComponent;
	[SerializeField] SoundComponent _natureAtmosphereSoundComponent;
	[SerializeField] SoundComponent _windSoundComponent;
	[SerializeField] SoundComponent _musicSoundComponent;
	
	[SerializeField] Sound _riverSFX;
	[SerializeField] Sound _natureAtmosphereSFX;
	[SerializeField] Sound _windSFX;
	[SerializeField] Sound _musicSFX;
		
	private void OnEnable()
	{
		PlaySounds();
	}
	
	private void OnDisable()
	{
		StopSounds();
	}

	private void PlaySounds()
	{
		_riverSoundComponent.PlaySound(_riverSFX);
		_natureAtmosphereSoundComponent.PlaySound(_natureAtmosphereSFX);
		_windSoundComponent.PlaySound(_windSFX);
		_musicSoundComponent.PlaySound(_musicSFX);
	}
	
	private void StopSounds()
	{
		_riverSoundComponent.StopSound();
		_natureAtmosphereSoundComponent.StopSound();
		_windSoundComponent.StopSound();
		_musicSoundComponent.StopSound();
	}
}
