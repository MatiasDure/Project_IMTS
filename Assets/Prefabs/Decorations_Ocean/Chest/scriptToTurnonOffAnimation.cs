using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chest : MonoBehaviour
{
    
    [SerializeField] private AnimationClip animation1; // First animation clip
    [SerializeField] private AnimationClip animation2; // Second animation clip

    private Animator animator;

    void Start()
    {
        // Get the Animator component on this GameObject
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Check for key presses to play animations
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            PlayAnimation(animation1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            PlayAnimation(animation2);
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
