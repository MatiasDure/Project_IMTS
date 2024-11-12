using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;

public class BeeMovementTest : MonoBehaviour
{
    private GameObject _beeCompanion;
    private BeeMovement _beeMovement;
    private GameObject _target;
    
    [SetUp]
    public void SetUp()
    {
        _beeCompanion = new GameObject();
        _beeMovement = _beeCompanion.AddComponent<BeeMovement>();
        _beeMovement._beeMovementStat.MovementSpeed = 50;
        _beeMovement._portalLayerMask = 1 >> 6;
        _target = new GameObject();
    }
    
    [UnityTest]
    public IEnumerator BeeMoveTowardPositionTest()
    {
        _target.transform.position = new Vector3(10, 10, 10);
        
        // Mock passage of time by manually calling LateUpdate
        float simulatedTime = 40f;
        float fixedDeltaTime = 1f / 60f; // Fixed deltaTime for each iteration (60 FPS equivalent)
        int steps = Mathf.CeilToInt(simulatedTime / fixedDeltaTime);

        for (int i = 0; i < steps; i++) // Around 2500 frame updates
        {
           _beeMovement.MoveTowardPosition(_target.transform.position,Vector3.zero);
        }
        
        Assert.IsTrue(MathHelper.AreVectorApproximatelyEqual(_beeCompanion.transform.position,_target.transform.position));
        yield return null;
    }
    
    [UnityTest]
    public IEnumerator BeeMoveTowardPositionThroughPortalTest()
    {
        GameObject portal = new GameObject();
        portal.transform.position = new Vector3(2, 2, 2);
        GameObject targetPortal = new GameObject();
        targetPortal.transform.position = new Vector3(5, 5, 5);
        
        _target.transform.position = new Vector3(12, 12, 10);
        
        // Mock passage of time by manually calling LateUpdate
        float simulatedTime = 40f;
        float fixedDeltaTime = 1f / 60f; // Fixed deltaTime for each iteration (60 FPS equivalent)
        int steps = Mathf.CeilToInt(simulatedTime / fixedDeltaTime);

        for (int i = 0; i < steps; i++) // Around 2500 frame updates
        {
            _beeMovement.MoveTowardPositionThroughPortal(portal.transform, targetPortal.transform, _target.transform,
                Vector3.zero);
        }
        
        Assert.IsTrue(MathHelper.AreVectorApproximatelyEqual(_beeCompanion.transform.position,_target.transform.position));
        yield return null;
    }
}
