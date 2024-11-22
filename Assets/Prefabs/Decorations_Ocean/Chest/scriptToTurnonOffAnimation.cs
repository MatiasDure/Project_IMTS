using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chest : MonoBehaviour
{
    
    [SerializeField] private AnimationClip animation1; // First animation clip
    [SerializeField] private AudioSource audioSource1; // First audio clip
    [SerializeField] private AnimationClip animation2; // Second animation clip
    [SerializeField] private AudioSource audioSource2; // Second audio clip

    private Animator animator;
    private AudioSource audioSource;

    void Start()
    {
        // Get the Animator component on this GameObject
        animator = GetComponent<Animator>();

        // Ensure AudioSources are assigned (optional error checking)
        if (audioSource1 == null || audioSource2 == null)
        {
            Debug.LogError("Please assign both AudioSource references in the inspector.");
        }
    }

    void Update()
    {
        // Check for key presses to play animations
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            PlayAnimation(animation1);
            audioSource1.Play();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            PlayAnimation(animation2);
            audioSource2.Play();
        }
    }

    // Method to play a specific animation clip
    private void PlayAnimation(AnimationClip clip)
    {
        if (animator != null && clip != null)
        {
            animator.Play(clip.name);
        }
    }

}
