using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;


public class PortalFunctionTest
{
   private GameObject _mainCamHolder;
   private Transform _mainCameraTransform;
   private GameObject _portalParent;
   private GameObject _anchor;
   private GameObject _secondCameraHolder;
   private PortalCameraMovementBehaviour _syncCamComponent;
   private GameObject _mainCamRaycaster;
   private GameObject _secondCamRaycaster;
   private GameObject _manager;
   private RaycastManager _raycastManager;
   

   [SetUp]
   public void SetUp()
   {
      // Create shared objects that are needed for multiple tests
      _mainCamHolder = new GameObject();
      _mainCameraTransform = _mainCamHolder.transform;
      _mainCameraTransform.position = Vector3.zero;
      _mainCameraTransform.rotation = Quaternion.identity;

      _portalParent = new GameObject();
      _portalParent.transform.position = new Vector3(0, -2, 0);
      _portalParent.transform.eulerAngles = new Vector3(-90, 0, -90);
      GameObject portalPlane = new GameObject();
      portalPlane.transform.SetParent(_portalParent.transform);

      _anchor = new GameObject();
      _anchor.transform.position = new Vector3(10, 10, 10);
      _anchor.transform.eulerAngles = new Vector3(-90, 180, 0);

      _secondCameraHolder = new GameObject();
      _secondCameraHolder.transform.position = new Vector3(10, 10, 10);
      _secondCameraHolder.transform.rotation = Quaternion.identity;
      _syncCamComponent = _secondCameraHolder.AddComponent<PortalCameraMovementBehaviour>();
      _syncCamComponent._portal = _portalParent;
      _syncCamComponent._worldAnchor = _anchor;

      _mainCamRaycaster = new GameObject();
      _mainCamRaycaster.transform.SetParent(_mainCamHolder.transform);
      _mainCamRaycaster.transform.position = Vector3.zero;
      _mainCamRaycaster.transform.rotation = Quaternion.identity;

      _secondCamRaycaster = new GameObject();
      _secondCamRaycaster.transform.SetParent(_secondCameraHolder.transform);
      _secondCamRaycaster.transform.position = Vector3.zero;
      _secondCamRaycaster.transform.rotation = Quaternion.identity;

      _manager = new GameObject();
      _raycastManager = _manager.AddComponent<RaycastManager>();
      _raycastManager._secondaryCamera = _secondCameraHolder;
   }
   
   [UnityTest]
   public IEnumerator Portal_Movement_Sync_Test()
   {
      // Mock passage of time by manually calling LateUpdate
      float simulatedTime = 40f;
      float fixedDeltaTime = 1f / 60f; // Fixed deltaTime for each iteration (60 FPS equivalent)
      int steps = Mathf.CeilToInt(simulatedTime / fixedDeltaTime);

      for (int i = 0; i < steps; i++) // Around 2500 frame updates
      {
         _syncCamComponent.UpdatePortalTransform(_anchor,_portalParent,_mainCamHolder);
      }

      Assert.IsTrue(CheckCameraTransform(_secondCameraHolder.transform, _anchor, _portalParent, _mainCamHolder));
      
      yield return null;
      
   }
   
   private bool CheckCameraTransform(Transform secondCamTransform, GameObject anchor, GameObject portal, GameObject mainCameraHolder)
   {
      var adjustedPositionAndRotationMatrix = anchor.transform.localToWorldMatrix * portal.transform.worldToLocalMatrix *
              mainCameraHolder.transform.localToWorldMatrix;

      return MathHelper.AreVectorApproximatelyEqual(secondCamTransform.position, adjustedPositionAndRotationMatrix.GetColumn(3))
             && secondCamTransform.rotation == adjustedPositionAndRotationMatrix.rotation;
   }
   
   [UnityTest]
   public IEnumerator Portal_Raycast_Test()
   {
      Ray mainCameraRay = new Ray(_mainCamHolder.transform.position,
                                    new Vector3(-0.95f, -0.05f, 0.30f));

      Ray secondCameraRay = _raycastManager.SecondCameraRay(mainCameraRay,_mainCamRaycaster,_secondCamRaycaster);

      Assert.IsTrue(secondCameraRay.direction == _mainCamRaycaster.transform.forward);
      
      yield return null;
   }
   
}
