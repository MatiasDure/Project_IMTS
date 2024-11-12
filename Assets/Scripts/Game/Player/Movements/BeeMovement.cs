using System;
using UnityEngine;

public class BeeMovement : MonoBehaviour
{
    [SerializeField] internal Movement _beeMovementStat;
    [SerializeField] internal LayerMask _portalLayerMask;
    private bool _overPortal;
    private bool _castRay;
    private RaycastHit _targetRaycastHit;
    private GameObject _hitPointObject;

    public static event Action BeeMovementEnd;

    private void Start()
    {
        SetUp();
        SubscribeToEvents();
    }

    private void SetUp()
    {
        _targetRaycastHit = default;
        _hitPointObject = new GameObject();
        _hitPointObject.name = "RaycastHitforPortal";
    }
    
    /// <summary>
    /// Moves the object towards a target position with an offset, checking if the target is reached,
    /// and triggers the movement end event when the target is reached.
    /// </summary>
    /// <param name="targetPosition">The position the object is moving towards.</param>
    /// <param name="targetOffset">The offset added to the target position to modify the movement target.</param>
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
    
    /// <summary>
    /// Moves the object towards the target through a portal, adjusting its position based on a raycast from the portal.
    /// If the raycast hits an object, the movement is adjusted to the hit point; otherwise, it moves directly from the portal to the target.
    /// </summary>
    /// <param name="portal">The current portal's transform (starting point for the raycast).</param>
    /// <param name="targetPortal">The target portal's transform (destination for the movement).</param>
    /// <param name="target">The target transform to move towards.</param>
    /// <param name="targetOffset">The offset applied to the target position during movement.</param>
    public void MoveTowardPositionThroughPortal(Transform portal, Transform targetPortal, Transform target, Vector3 targetOffset)
    {
        if (!_castRay)
        {
            _castRay = true;
            CastingRaycastThroughPortal(portal, targetPortal, target, out _targetRaycastHit);
            
            if(_targetRaycastHit.collider != null)
                _hitPointObject.transform.position = _targetRaycastHit.point;
        }
        
        if (_targetRaycastHit.collider != null)
            MoveThroughPortal(AdjustPosition(targetPortal,portal,_hitPointObject.transform),
                _targetRaycastHit.point,
                target.position,
                targetOffset);
        else
            MoveThroughPortal(portal.position, 
                targetPortal.position, 
                target.position,
                targetOffset);
    }

    private void MoveThroughPortal(Vector3 portalPosition, Vector3 targetPortalPos, Vector3 targetPosition, Vector3 targetOffset)
    {
        Vector3 thisTransformPosition = transform.position;
        
        bool reachTarget = MathHelper.AreVectorApproximatelyEqual(thisTransformPosition, targetPosition,0.1f);
        bool isAtPortal = MathHelper.AreVectorApproximatelyEqual(thisTransformPosition, portalPosition,0.1f);
        
        if (reachTarget)
        {
            OnBeeMovementEnd();
            return;
        }
        
        if (!isAtPortal && !_overPortal)
            MoveToPortalPosition(portalPosition);
        else if (isAtPortal)
            EnterPortal(targetPortalPos);
        else
            MoveTowardPosition(targetPosition, targetOffset);
    }
    
    private void MoveToPortalPosition(Vector3 portalPosition)
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            portalPosition,
            _beeMovementStat.MovementSpeed * Time.deltaTime
        );
    }
    
    private void EnterPortal(Vector3 targetCameraPosition)
    {
        transform.position = targetCameraPosition;
        _overPortal = true;
    }

    private void CastingRaycastThroughPortal(Transform portal, Transform targetPortal, Transform target,
        out RaycastHit hit)
    {
        Vector3 stimulateObjectPosition = AdjustPosition(portal, targetPortal,transform);

        int layerMask = _portalLayerMask;

        Vector3 targetPosition = target.position;
        Vector3 direction = (stimulateObjectPosition - targetPosition).normalized;
        float distance = Vector3.Distance(targetPosition, stimulateObjectPosition);

        Physics.Raycast(targetPosition, direction, out hit, distance, layerMask);
    }

    private Vector3 AdjustPosition(Transform portal, Transform target, Transform toAdjust)
    {
        var adjustedPositionAndRotationMatrix = target.transform.localToWorldMatrix
                                                * portal.transform.worldToLocalMatrix
                                                * toAdjust.localToWorldMatrix;

        Vector3 stimulatePosition = adjustedPositionAndRotationMatrix.GetColumn(3);
        return stimulatePosition;
    }
    
    private void HanddleDone()
    {
        _targetRaycastHit = default; 
        _castRay = false;
        _overPortal = false;
    }
    private void SubscribeToEvents()
    {
        BeeMovementEnd += HanddleDone;
    }
    private void UnSubscribeToEvents()
    {
        BeeMovementEnd -= HanddleDone;
    }
    private static void OnBeeMovementEnd()
    {
        BeeMovementEnd?.Invoke();
    }
    private void OnDestroy()
    {
        UnSubscribeToEvents();
    }
}
