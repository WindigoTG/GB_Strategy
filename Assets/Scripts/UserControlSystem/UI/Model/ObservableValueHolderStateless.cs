using System;
using UniRx;

public class ObservableValueHolderStateless<T> : ValueHolderBase<T>, IObservable<T>
{
    private Subject<T> _observableValue = new Subject<T>();

    public override void SetValue(T value)
    {
        base.SetValue(value);
        _observableValue.OnNext(value);
    }

    public IDisposable Subscribe(IObserver<T> observer)
    {
        return _observableValue.Subscribe(observer);
    }
}

