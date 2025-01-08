using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageBackgroundSounds : MonoBehaviour
{
	[SerializeField] SoundComponent _riverSoundComponent;
	[SerializeField] SoundComponent _natureAtmosphereSoundComponent;
	[SerializeField] SoundComponent _windSoundComponent;
	
	[SerializeField] Sound _riverSFX;
	[SerializeField] Sound _natureAtmosphereSFX;
	[SerializeField] Sound _windSFX;
	
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
	}
	
	private void StopSounds()
	{
		_riverSoundComponent.StopSound();
		_natureAtmosphereSoundComponent.StopSound();
		_windSoundComponent.StopSound();
	}
}
