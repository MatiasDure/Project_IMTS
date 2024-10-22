using UnityEngine;

public class PlayParticle : MonoBehaviour, IInteractable
{
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private Transform _origin;

    private ParticleSystem _instatiateSystem;
    public void Interact()
    {
        PlaySystem();
    }

    private void PlaySystem()
    {
        if (_instatiateSystem == null)
        {
            _instatiateSystem = Instantiate(_particleSystem, _origin);
            
            var transform1 = _instatiateSystem.transform;
            transform1.localPosition = Vector3.zero;
            transform1.localRotation = Quaternion.identity;
            transform1.localScale = Vector3.one;
        }
        
        _instatiateSystem.Play();
    }
}
