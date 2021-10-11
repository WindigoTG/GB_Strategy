using UnityEngine;
using System.Threading;

public class StopCommandExecutor : CommandExecutorBase<IStopCommand>
{
    private CancellationTokenSource _ctSource = new CancellationTokenSource();

    public override void ExecuteSpecificCommand(IStopCommand command)
    {
        _ctSource.Cancel();
        _ctSource = null;

        Debug.Log($"<color=#FF00FF>{name} has stopped</color>");
    }

    public CancellationToken CToken
    {
        get
        {
            _ctSource = new CancellationTokenSource();

            return _ctSource.Token;
        }
    }
}
