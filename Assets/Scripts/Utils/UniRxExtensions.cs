using System;
using UniRx;

public static class UniRxExtensions 
{
    public static IDisposable Subscribe<T1>(this IObservable<CollectionAddEvent<T1>> source, Action<T1, int> onNext) =>
        ObservableExtensions.Subscribe(source, t => onNext(t.Value, t.Index));
}
