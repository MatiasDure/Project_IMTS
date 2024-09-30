using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip buttonPress;
    [SerializeField] private AudioClip badge;
    [SerializeField] private AudioClip happy;
    [SerializeField] private AudioClip sad;


    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    public void PlayButtonSound()
    {
        source.PlayOneShot(buttonPress);
    }
    public void PlayBadgeSound()
    {
        source.PlayOneShot(badge);
    }
    public void PlayHappySound()
    {
        source.PlayOneShot(happy);
    }

    public void PlaySadSound()
    {
        source.PlayOneShot(sad);
    }
}
