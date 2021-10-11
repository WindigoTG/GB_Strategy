using UnityEngine;

[RequireComponent(typeof(SetRallyPointCommandExecutor))]
public class RemoveRallyPointCommandExecutor : CommandExecutorBase<IRemoveRallyPointCommand>
{
    private IRallyPointHolder _rallyPoint;

    private void Awake()
    {
        _rallyPoint = GetComponent<SetRallyPointCommandExecutor>();
    }

    public override void ExecuteSpecificCommand(IRemoveRallyPointCommand command)
    {
        _rallyPoint.ResetRallyPoint();
    }
}
