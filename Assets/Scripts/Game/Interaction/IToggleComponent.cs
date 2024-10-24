
public interface IToggleComponent
{
    public ToggleState toggleState{ get; set; }
    public bool ignoreInput { get; set; }
    
    public void ToggleOn();

    public void ToggleOff();

    public void OnSwitchState();

}
