using System;


public abstract class ValueHolderBase<T> : IAwaitable<T>
{
    public class NewValueNotifier<TAwaited> : AwaiterBase<TAwaited>
    {
        private readonly ValueHolderBase<TAwaited> _objectValueBase;

        public NewValueNotifier(ValueHolderBase<TAwaited> objectValueBase)
        {
            _objectValueBase = objectValueBase;
            _objectValueBase.OnNewValue += OnAwaitedEvent;
        }

        protected override void OnAwaitedEvent(TAwaited obj)
        {
            _objectValueBase.OnNewValue -= OnAwaitedEvent;
            base.OnAwaitedEvent(obj);
        }
    }

    public T CurrentValue { get; private set; }
	public Action<T> OnNewValue;

	public virtual void SetValue(T value)
	{
		CurrentValue = value;
		OnNewValue?.Invoke(value);
	}

    public IAwaiter<T> GetAwaiter()
    {
        return new NewValueNotifier<T>(this);
    }
}
