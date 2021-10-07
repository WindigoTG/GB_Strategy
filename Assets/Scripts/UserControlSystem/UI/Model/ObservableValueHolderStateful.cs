using System;
using UniRx;

public class ObservableValueHolderStateful<T> : ValueHolderBase<T>, IObservable<T>
{
    private ReactiveProperty<T> _observableValue = new ReactiveProperty<T>();

    public override void SetValue(T value)
    {
        base.SetValue(value);
        _observableValue.Value = value;
    }

    public IDisposable Subscribe(IObserver<T> observer)
    {
        return _observableValue.Subscribe(observer);
    }
}
