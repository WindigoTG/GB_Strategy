using UnityEngine;
using System.Threading;
using System.Threading.Tasks;

public class StopCommandExecutor : CommandExecutorBase<IStopCommand>
{
    private CancellationTokenSource _ctSource;

    public override async Task ExecuteSpecificCommand(IStopCommand command)
    {
        if (_ctSource == null)
            return;

        _ctSource.Cancel();
        _ctSource = null;

        Debug.Log($"<color=#FF00FF>{name} has stopped</color>");
        await Task.CompletedTask;
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
