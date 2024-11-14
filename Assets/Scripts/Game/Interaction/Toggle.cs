using UnityEngine;

[RequireComponent(typeof(IToggleComponent))]
public class Toggle : MonoBehaviour, IInteractable
{
    private IToggleComponent[] _toggleComponent;

	public bool CanInterrupt { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
	public bool MultipleInteractions { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

	public void Start()
    {
        _toggleComponent = GetComponents<IToggleComponent>();

        if (_toggleComponent == null || _toggleComponent.Length <= 0)
        {
            throw new System.Exception("No toggle component found. Please assign it in the Inspector.");
        }
    }
    
    public void Interact()
    {
        OnToggle();
    }
    
    internal void OnToggle()
    {
        foreach (var component in _toggleComponent)
        {
            component.Toggle();
        }
    }
}
