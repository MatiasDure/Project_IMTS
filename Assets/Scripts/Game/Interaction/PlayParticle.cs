using UnityEngine;

public class PlayParticle : MonoBehaviour, IInteractable
{
    [SerializeField] internal ParticleSystem _particleSystem;
    [SerializeField] internal Transform _origin;

    internal ParticleSystem _instatiateSystem;
    public void Interact()
    {
        PlaySystem();
    }

    internal void PlaySystem()
    {
        if (_instatiateSystem == null)
        {
            _instatiateSystem = Instantiate(_particleSystem, _origin);
            
            var systemTransform = _instatiateSystem.transform;
            systemTransform.SetLocalPositionAndRotation(Vector3.zero,Quaternion.identity);
            systemTransform.localScale = Vector3.one;
        }
        
        _instatiateSystem.Play();
    }
}
