using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseAnimation : MonoBehaviour
{
    [SerializeField] AnimationCurve anim;
    [SerializeField] float duration;

    private float timer;
    private Vector3 origScale;
    private bool isAnimating = false;

    private void Start()
    {
        origScale = transform.localScale;
    }

    private void Update()
    {
        if (isAnimating)
        {
            timer += Time.deltaTime;
            float progress = timer / duration;

            float scaleValue = anim.Evaluate(progress);
            transform.localScale = origScale * scaleValue;

            if(timer >= duration)
            {
                isAnimating = false;
                transform.localScale = origScale;
            }
        }
    }

    public void TriggerAnimation()
    {
        if (isAnimating) return;

        isAnimating = true;
        timer = 0.0f;
        FindObjectOfType<AudioManager>().Play("Button");
    }
}
