using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private AudioSource _source;
    [SerializeField] private AudioClip buttonPress;
    [SerializeField] private AudioClip talk;
    [SerializeField] private AudioClip badge;
    [SerializeField] private AudioClip happy;
    [SerializeField] private AudioClip sad;


    private void Awake()
    {
        _source = GetComponent<AudioSource>();
    }

    public void PlayButtonSound()
    {
        _source.PlayOneShot(buttonPress);
    }
    public void PlayBadgeSound()
    {
        _source.PlayOneShot(badge);
    }

    public void PlayTalkSound()
    {
        _source.PlayOneShot(talk);
    }
    public void PlayHappySound()
    {
        _source.PlayOneShot(happy);
    }

    public void PlaySadSound()
    {
        _source.PlayOneShot(sad,1.5f);
    }
}
