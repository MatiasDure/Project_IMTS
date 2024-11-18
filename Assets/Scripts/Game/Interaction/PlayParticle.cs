using System;
using UnityEngine;

public class PlayParticle : MonoBehaviour //, IToggleComponent
{
    //public ToggleState CurrentToggleState { get; set; }

    //public bool ignoreInput { get; set; }
	//public ToggleState NextToggleState { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

	[SerializeField] internal ParticleSystem _particleSystemprefab;
    [SerializeField] internal Transform _origin;

    internal ParticleSystem _particleSystem;

	public event Action OnToggleDone;

	private void Start()
    {
        // CurrentToggleState = ToggleState.Off;
        // ignoreInput = false;
        
        _particleSystem = InstantiateParticleSystem(_particleSystem,_particleSystemprefab,_origin);
    }

    // public void Toggle()
    // {
    //     if(ignoreInput) return;
        
    //     //interact
    //     if (CurrentToggleState == ToggleState.Off) ToggleOn(); 
    //     else ToggleOff();
    // }

    public void ToggleOn()
    {
        _particleSystem.Play();
        // UpdateState(ToggleState.On);
    }

    public void ToggleOff()
    {
        if(_particleSystem.isPlaying) _particleSystem.Stop();
        // UpdateState(ToggleState.Off);
    }

    // private void UpdateState(ToggleState state)
    // {
    //     CurrentToggleState = state;
    // }
    
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
