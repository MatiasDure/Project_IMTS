
public interface IToggleComponent
{
    public ToggleState toggleState{ get; internal set; }
    public bool ignoreInput { get; internal set; }
    
    public void ToggleOn();

    public void ToggleOff();

    public void Toggle();

}
