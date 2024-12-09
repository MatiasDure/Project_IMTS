using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundComponent : MonoBehaviour
{
	private AudioSource _audioSource;
	
	private void Awake()
	{
		_audioSource = GetComponent<AudioSource>();
	}
	
	public void PlaySound(Sound sound)
	{
		_audioSource.clip = sound.clip;
		_audioSource.volume = sound.volume;
		_audioSource.priority = sound.priority;
		_audioSource.loop = sound.loop;
		
		if(sound.loop)
			_audioSource.Play();
		else
			_audioSource.PlayOneShot(sound.clip, sound.volume);
	}
	
	public void StopSound()
	{
		_audioSource.Stop();
	}
}
