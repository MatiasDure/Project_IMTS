using UnityEngine;

public class Toggle : MonoBehaviour, IInteractable
{   
    private ToggleState _toggleState;
    private IToggleComponent _toggleComponent;
    
    public void Start()
    {
        _toggleComponent = GetComponent<IToggleComponent>();
        _toggleState = ToggleState.ToggleOff;
    }

    public void Interact()
    {
        if (_toggleComponent == null) return;
        if (_toggleComponent.ignoreInput) return;

        ChangeState();
    }

    private void ChangeState()
    {
        switch(_toggleState) {
            case ToggleState.ToggleOff:
                _toggleComponent.ToggleOn();
                _toggleState = ToggleState.ToggleOn;
                break;
            case ToggleState.ToggleOn:
                _toggleComponent.ToggleOff();
                _toggleState = ToggleState.ToggleOff;
                break;
            default:
                _toggleComponent.ToggleOn();
                _toggleState = ToggleState.ToggleOn;
                break;
        }
    }

}
