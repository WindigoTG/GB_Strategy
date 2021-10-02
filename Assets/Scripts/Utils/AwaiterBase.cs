using System;

public abstract class AwaiterBase<TAwaited> : IAwaiter<TAwaited>
{
    private Action _continuation;
    private bool _isCompleted;
    private TAwaited _result;

    public void OnCompleted(Action continuation)
    {
        if (_isCompleted)
        {
            continuation?.Invoke();
        }
        else
        {
            _continuation = continuation;
        }
    }

    protected virtual void OnAwaitedEvent(TAwaited obj)
    {
        _result = obj;
        _isCompleted = true;
        _continuation?.Invoke();
    }

    public bool IsCompleted => _isCompleted;

    public TAwaited GetResult() => _result;
}
