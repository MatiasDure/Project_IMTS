using System;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Toggle))]
public class PlayParticle : MonoBehaviour, IToggleComponent
{
    internal ToggleState toggleState { get; set; }
    
    ToggleState IToggleComponent.toggleState
    {
        get => toggleState;
        set => toggleState = value;
    }

    internal bool ignoreInput { get; set; }

    bool IToggleComponent.ignoreInput
    {
        get => ignoreInput;
        set => ignoreInput = value;
    }

    [SerializeField] internal ParticleSystem _particleSystemprefab;
    [SerializeField] internal Transform _origin;

    internal ParticleSystem _particleSystem;

    private void Start()
    {
        toggleState = ToggleState.ToggleOff;
        ignoreInput = false;
        
        _particleSystem = InstantiateParticleSystem(_particleSystem,_particleSystemprefab,_origin);
    }

    public void Toggle()
    {
        if(ignoreInput) return;
        //update state
        toggleState = toggleState == ToggleState.ToggleOff ? ToggleState.ToggleOn : ToggleState.ToggleOff;
        //interact
        if (toggleState == ToggleState.ToggleOn) ToggleOn(); 
        else ToggleOff();
    }

    public void ToggleOn()
    {
        _particleSystem.Play();
    }

    public void ToggleOff()
    {
        if(_particleSystem.isPlaying) _particleSystem.Stop();
    }
    
    internal ParticleSystem InstantiateParticleSystem(ParticleSystem instatiateSystem, ParticleSystem particleSystemToInstantiate, Transform parent)
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
