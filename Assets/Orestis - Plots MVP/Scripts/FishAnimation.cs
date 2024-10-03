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
    [SerializeField] private float maxDistance;
    [SerializeField] private Transform resetPos;

    private bool canSpeedUp = true;
    private bool canCheckPos = true;

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

        Vector3 planePos = bounds.center;
        planePos.y = transform.position.y;
        Vector3 deltaVec = transform.position - planePos;
        float moveDelta = deltaVec.magnitude;
        // TEMP
        float moveMargin = maxDistance;

        // Temporary - To prevent wrong teleports
        if (!canCheckPos)
        {
            if(moveDelta < maxDistance * 1)
            {
                canCheckPos = true;
            }
            else
            {
                return;
            }
        }

        if (moveDelta >= moveMargin)
        {
            ResetPosition();
        }

    }

    // Switch movement direction
    private void ResetPosition()
    {
        canCheckPos = false;

        transform.position = resetPos.position;
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
