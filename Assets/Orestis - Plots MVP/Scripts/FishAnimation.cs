using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishAnimation : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float speedUpTimeSec = 1.5f;
    [SerializeField] private float speedUpMultiplier = 3f;
    [SerializeField] private Transform plane;
    [SerializeField] private BoxCollider col;
    [SerializeField] private MeshCollider meshCol;

    private bool canSpeedUp = true;

    private void Update()
    {
        Move();
        CheckPosition();
    }

    private void Move()
    {
        transform.position += -transform.right * moveSpeed * Time.deltaTime;
    }

    private void CheckPosition()
    {
        Bounds bounds = meshCol.bounds;
        
        float planePos = bounds.center.x;
        float halfPlaneWidth = bounds.size.x / 2;

        //FOR MVP
        float fishWidth = (col.size.x * transform.localScale.x) / 4;
        //AFTER MVP
        //float fishWidth = (col.size.x * transform.localScale.x) / 2;

        float moveMargin = planePos + halfPlaneWidth + fishWidth;

        //Debug.Log($"Local Position: {transform.localPosition}, Move Margin: {moveMargin}");
        if (transform.position.x >= moveMargin)
        {
            ResetPosition(planePos, halfPlaneWidth, fishWidth);
        }
    }

    // Switch movement direction
    private void ResetPosition(float planePos, float halfPlaneWidth, float fishWidth)
    {
        Vector3 newPos = transform.position;
        newPos.x = planePos - halfPlaneWidth - fishWidth;

        transform.position = newPos;
    }

    public void IncreaseSpeed()
    {
        if (!canSpeedUp) return;
        canSpeedUp = false;

        FindObjectOfType<AudioManager>().Play("Bubbles");
        StartCoroutine(IncreaseSpeedCoroutine());
    }

    IEnumerator IncreaseSpeedCoroutine()
    {
        float origSpeed = moveSpeed;
        moveSpeed *= speedUpMultiplier;

        yield return new WaitForSeconds(speedUpTimeSec);
        canSpeedUp = true;
        moveSpeed = origSpeed;
    }
}
