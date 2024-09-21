using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class InteractableSound : MonoBehaviour
{
    [SerializeField]
    private AudioClip _destroyedClip;

    public Action OnSoundFinished;

    public IEnumerator PlaySound()
    {
        var audioSource = GetComponent<AudioSource>();
        audioSource.clip = _destroyedClip;
        audioSource.Play();
        
        yield return new WaitForSeconds(_destroyedClip.length);

        OnSoundFinished?.Invoke();
    }

}
