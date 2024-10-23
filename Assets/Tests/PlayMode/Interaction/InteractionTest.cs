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
        SetUpGameObject();
        
        //if failed increase the rotateSpeed
        ToggleRotate toggleRotateComponent = _interactionHolder.AddComponent<ToggleRotate>();
        toggleRotateComponent._openAngle = 90f;
        toggleRotateComponent._rotateSpeed = 1e+20f;
        toggleRotateComponent._rotationAxis = ToggleRotate.RotationAxis.y;

        toggleRotateComponent.ToggleOn();
        yield return null;
        Assert.AreEqual(new Vector3(0,toggleRotateComponent._openAngle,0),
            _interactionHolder.transform.eulerAngles);

        yield return null;
    }
    [UnityTest]
    public IEnumerator RotateOffInteractionTest()
    {
        SetUpGameObject();
        
        //if failed increase the rotateSpeed
        _interactionHolder.transform.eulerAngles = new Vector3(0, 90, 0);
        
        ToggleRotate toggleRotateComponent = _interactionHolder.AddComponent<ToggleRotate>();
        toggleRotateComponent._openAngle = 90f;
        toggleRotateComponent._rotateSpeed = 1e+20f;
        toggleRotateComponent._rotationAxis = ToggleRotate.RotationAxis.y;
        
        toggleRotateComponent.ToggleOff();
        yield return null;
        Assert.AreEqual(new Vector3(0,0,0),
            _interactionHolder.transform.eulerAngles);
        
        yield return null;
    }

    [UnityTest]
    public IEnumerator ParticleTest()
    {
        SetUpGameObject();

        GameObject systemOrigin = new GameObject();
        GameObject particleHolder = new GameObject();
        particleHolder.AddComponent<ParticleSystem>();
        
        PlayParticle playParticleComponent = _interactionHolder.AddComponent<PlayParticle>();
        playParticleComponent._particleSystem = particleHolder.GetComponent<ParticleSystem>();
        playParticleComponent._origin = systemOrigin.transform;

        playParticleComponent.PlaySystem();
        
        Assert.IsNotNull(playParticleComponent._instatiateSystem);
        Assert.AreEqual(systemOrigin.transform.position,
            playParticleComponent._instatiateSystem.transform.position);
        
        yield return null;
    }
}
