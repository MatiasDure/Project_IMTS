using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class BeeMovement : MonoBehaviour
{
    [SerializeField] private Movement _beeMovementStat;
    
    private bool _overPortal;

    public static event Action BeeMovementEnd; 

    public void MoveTowardPosition(Vector3 targetPosition, Vector3 targetOffset)
    {
        bool reachTarget = MathHelper.AreVectorApproximatelyEqual(transform.position, targetPosition,0.1f);

        if (!reachTarget)
        {
            transform.position = Vector3.MoveTowards(transform.position, 
                targetPosition + targetOffset, 
                _beeMovementStat.MovementSpeed * Time.deltaTime);
        }
        else
        {
            OnBeeMovementEnd();
        }
    }

    public void MoveTowardPositionThroughPortal(Vector3 portalPosition, Vector3 targetPortalPos, Vector3 targetPosition, Vector3 targetOffset)
    {
        bool reachTarget = MathHelper.AreVectorApproximatelyEqual(transform.position, targetPosition,0.1f);
        bool isAtPortal = MathHelper.AreVectorApproximatelyEqual(transform.position, portalPosition,0.1f);
        
        if (reachTarget)
        {
            _overPortal = false;
            OnBeeMovementEnd();
            return;
        }
        
        if (!isAtPortal && !_overPortal)
        {
            MoveToPortalPosition(portalPosition);
        }
        else if (isAtPortal)
        {
            EnterPlot(targetPortalPos);
        }
        else
        {
            MoveTowardPosition(targetPosition, targetOffset);
        }
    }
    
    private void MoveToPortalPosition(Vector3 portalPosition)
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            portalPosition,
            _beeMovementStat.MovementSpeed * Time.deltaTime
        );
    }
    
    private void EnterPlot(Vector3 targetCameraPosition)
    {
        transform.position = targetCameraPosition;
        _overPortal = true;
    }

    private static void OnBeeMovementEnd()
    {
        BeeMovementEnd?.Invoke();
    }
}
