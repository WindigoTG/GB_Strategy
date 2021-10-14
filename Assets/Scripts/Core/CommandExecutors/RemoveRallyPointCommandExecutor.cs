using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(SetRallyPointCommandExecutor))]
public class RemoveRallyPointCommandExecutor : CommandExecutorBase<IRemoveRallyPointCommand>
{
    private IRallyPointHolder _rallyPoint;

    private void Awake()
    {
        _rallyPoint = GetComponent<SetRallyPointCommandExecutor>();
    }

    public override async Task ExecuteSpecificCommand(IRemoveRallyPointCommand command)
    {
        _rallyPoint.ResetRallyPoint();
        await Task.CompletedTask;
    }
}
