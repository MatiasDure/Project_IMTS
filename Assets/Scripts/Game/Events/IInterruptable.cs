using System;

public interface IInterruptible
{
	public event Action<IInterruptible> OnInterruptedDone;
	// public void Invoke(IInterruptable interruptable) => OnInterruptedDone?.Invoke(interruptable);
    public void InterruptEvent();
}