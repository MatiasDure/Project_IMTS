using System;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Toggle))]
public class PlayParticle : MonoBehaviour, IToggleComponent
{
    public ToggleState toggleState { get; set; }

    public bool ignoreInput { get; set; }
    

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
        
        //interact
        if (toggleState == ToggleState.ToggleOff) ToggleOn(); 
        else ToggleOff();
    }

    public void ToggleOn()
    {
        _particleSystem.Play();
        UpdateState(ToggleState.ToggleOn);
    }

    public void ToggleOff()
    {
        if(_particleSystem.isPlaying) _particleSystem.Stop();
        UpdateState(ToggleState.ToggleOff);
    }

    private void UpdateState(ToggleState state)
    {
        toggleState = state;
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
