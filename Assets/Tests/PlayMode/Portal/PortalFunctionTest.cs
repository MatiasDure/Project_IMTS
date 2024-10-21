using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;

public class PortalFunctionTest
{
   [UnityTest]
   public IEnumerator Portal_Movement_Sync_Test()
   {
      GameObject mainCamHolder = new GameObject();
      var mainCameraTransform = mainCamHolder.transform;
      mainCameraTransform.position = Vector3.zero;
      mainCameraTransform.rotation = Quaternion.identity;
      
      GameObject portalParent = new GameObject();
      portalParent.transform.position = new Vector3(0, -2, 0);
      portalParent.transform.eulerAngles = new Vector3(-90, 0, -90);
      GameObject portalPlane = new GameObject();
      portalPlane.transform.SetParent(portalParent.transform);

      GameObject anchor = new GameObject();
      anchor.transform.position = new Vector3(10, 10, 10);
      anchor.transform.eulerAngles = new Vector3(-90, 180, 0);

      GameObject secondCameraHolder = new GameObject();
      var secondCameraTransform = secondCameraHolder.transform;
      secondCameraTransform.position = new Vector3(10, 10, 10);
      secondCameraTransform.rotation = Quaternion.identity;
      PortalCameraMovementBehaviour syncCamComponent = secondCameraHolder.AddComponent<PortalCameraMovementBehaviour>();
      syncCamComponent.portal = portalParent;
      syncCamComponent.worldAnchor = anchor;
      
      // Mock passage of time by manually calling LateUpdate
      float simulatedTime = 40f;
      float fixedDeltaTime = 1f / 60f; // Fixed deltaTime for each iteration (60 FPS equivalent)
      int steps = Mathf.CeilToInt(simulatedTime / fixedDeltaTime);

      for (int i = 0; i < steps; i++) // Around 2500 frame updates
      {
         syncCamComponent.UpdatePortalTransform(anchor,portalParent,mainCamHolder);
      }

      Assert.IsTrue(CheckCameraTransform(secondCameraTransform, anchor, portalParent, mainCamHolder));
      
      yield return null;
      
   }
   
   private bool CheckCameraTransform(Transform secondCamTransform, GameObject anchor, GameObject portal, GameObject mainCameraHolder)
   {
      var m = anchor.transform.localToWorldMatrix * portal.transform.worldToLocalMatrix *
              mainCameraHolder.transform.localToWorldMatrix;

      return secondCamTransform.position == (Vector3)m.GetColumn(3) && secondCamTransform.rotation == m.rotation;
   }
   
   [UnityTest]
   public IEnumerator Portal_Raycast_Test()
   {
      GameObject mainCamHolder = new GameObject();
      var mainCameraTransform = mainCamHolder.transform;
      mainCameraTransform.position = Vector3.zero;
      mainCameraTransform.rotation = Quaternion.identity;

      GameObject mainCamRaycaster = new GameObject();
      mainCamRaycaster.transform.SetParent(mainCamHolder.transform);
      mainCamRaycaster.transform.position = Vector3.zero;
      mainCamRaycaster.transform.rotation = Quaternion.identity;
      
      GameObject portalParent = new GameObject();
      portalParent.transform.position = new Vector3(0, -2, 0);
      portalParent.transform.eulerAngles = new Vector3(-90, 0, -90);
      GameObject portalPlane = new GameObject();
      portalPlane.transform.SetParent(portalParent.transform);

      GameObject anchor = new GameObject();
      anchor.transform.position = new Vector3(10, 10, 10);
      anchor.transform.eulerAngles = new Vector3(-90, 180, 0);

      GameObject secondCameraHolder = new GameObject();
      var secondCameraTransform = secondCameraHolder.transform;
      secondCameraTransform.position = new Vector3(10, 10, 10);
      secondCameraTransform.rotation = Quaternion.identity;

      GameObject secondCamRaycaster = new GameObject();
      secondCamRaycaster.transform.SetParent(secondCameraHolder.transform);
      secondCamRaycaster.transform.position = Vector3.zero;
      secondCamRaycaster.transform.rotation = Quaternion.identity;

      GameObject raycastManagerGameObject = new GameObject();
      RaycastManager raycastManager = raycastManagerGameObject.AddComponent<RaycastManager>();
      raycastManager._secondaryCamera = secondCameraHolder;

      Ray mainCameraRay = new Ray(mainCamHolder.transform.position,
                                    new Vector3(-0.95f, -0.05f, 0.30f));

      Ray secondCameraRay = raycastManager.SecondCameraRay(mainCameraRay,mainCamRaycaster,secondCamRaycaster);

      Assert.IsTrue(secondCameraRay.direction == mainCamRaycaster.transform.forward);
      
      yield return null;
      
   }
   
}
