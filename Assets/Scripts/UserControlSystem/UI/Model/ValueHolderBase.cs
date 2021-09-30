using System;

public class ValueHolderBase<T>
{
	public T CurrentValue { get; private set; }
	public Action<T> OnNewValue;

	public virtual void SetValue(T value)
	{
		CurrentValue = value;
		OnNewValue?.Invoke(value);
	}
}
