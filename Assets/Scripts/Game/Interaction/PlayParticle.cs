using System;
using UnityEngine;

[RequireComponent(typeof(Toggle))]
public class PlayParticle : MonoBehaviour, IToggleComponent
{
    public ToggleState toggleState { get; set; }
    public bool ignoreInput { get; set; }

    [SerializeField] internal ParticleSystem _particleSystem;
    [SerializeField] internal Transform _origin;

    internal ParticleSystem _instatiateSystem;

    private void Start()
    {
        toggleState = ToggleState.ToggleOff;
        ignoreInput = false;
        
        _instatiateSystem = InstantiateSystem(_instatiateSystem,_particleSystem,_origin);
    }

    public void OnSwitchState()
    {
        if(ignoreInput) return;
        switch (toggleState)
        {
            case ToggleState.ToggleOff:
                ToggleOn();
                toggleState = ToggleState.ToggleOn;
                break;
            case ToggleState.ToggleOn:
                ToggleOff();
                toggleState = ToggleState.ToggleOff;
                break;
            default:
                ToggleOn();
                toggleState = ToggleState.ToggleOn;
                break;
        }
    }
    public void ToggleOn()
    {
        _instatiateSystem.Play();
    }

    public void ToggleOff()
    {
        if(_instatiateSystem.isPlaying) _instatiateSystem.Stop();
    }

    internal ParticleSystem InstantiateSystem(ParticleSystem instatiateSystem, ParticleSystem particleSystemToInstantiate, Transform parent)
    {
        if (instatiateSystem == null)
        {
            instatiateSystem = Instantiate(particleSystemToInstantiate, parent);

            var systemTransform = instatiateSystem.transform;
            systemTransform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            systemTransform.localScale = Vector3.one;
            
            return instatiateSystem;
        }
        
        return instatiateSystem; 
    }
}
