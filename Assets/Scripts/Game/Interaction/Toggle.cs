using Codice.Client.BaseCommands.Update;
using UnityEngine;

[RequireComponent(typeof(IToggleComponent))]
public class Toggle : MonoBehaviour, IInteractable
{
    private IToggleComponent[] _toggleComponent;
    
    public void Start()
    {
        _toggleComponent = GetComponents<IToggleComponent>();

        if (_toggleComponent == null)
        {
            throw new System.Exception("No toggle component found. Please assign it in the Inspector.");
        }
    }
    
    public void Interact()
    {
        OnToggle();
    }
    
    public void OnToggle()
    {
        foreach (var component in _toggleComponent)
        {
            component.OnSwitchState();
        }
    }
}
