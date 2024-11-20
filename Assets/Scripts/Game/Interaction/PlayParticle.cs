using System;
using UnityEngine;

public class PlayParticle : MonoBehaviour 
{
    [SerializeField] internal ParticleSystem _particleSystemprefab;
    [SerializeField] internal Transform _origin;

    internal ParticleSystem _particleSystem;

    private void Start()
    {
        _particleSystem = InstantiateParticleSystem(_particleSystem,_particleSystemprefab,_origin);
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
