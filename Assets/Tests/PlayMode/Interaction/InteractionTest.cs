using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;

public class InteractionTest
{
    private GameObject _interactionHolder;
    

    [SetUp]
    public void SetUpGameObject()
    {
        _interactionHolder = new GameObject();
    }

    [UnityTest]
    public IEnumerator RotateOnInteractionTest()
    {
        //if failed increase the rotateSpeed
        ToggleRotate toggleRotateComponent = _interactionHolder.AddComponent<ToggleRotate>();
        toggleRotateComponent._openAngle = 90f;
        toggleRotateComponent._rotateSpeed = 1e+20f;
        toggleRotateComponent._rotationAxis = Axis.Y;

        Quaternion targetRotation = Quaternion.Euler(0, 90, 0);
        
        toggleRotateComponent.transform.rotation =
            toggleRotateComponent.RotateToTarget(toggleRotateComponent.transform.rotation,targetRotation,toggleRotateComponent._rotateSpeed);
       
        yield return null;
        
        Assert.AreEqual(targetRotation, _interactionHolder.transform.rotation);
    }
}
