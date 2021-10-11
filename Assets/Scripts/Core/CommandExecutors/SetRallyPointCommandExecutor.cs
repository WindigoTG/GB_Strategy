using UnityEngine;

public class SetRallyPointCommandExecutor : CommandExecutorBase<ISetRallyPointCommand>, IRallyPointHolder
{
    private Vector3 _rallyPoint;
    private bool _isSet;

    public Vector3 RallyPoint => _rallyPoint;

    public bool IsRallyPointSet => _isSet;

    public override void ExecuteSpecificCommand(ISetRallyPointCommand command)
    {
        _rallyPoint = command.Target;
        _isSet = true;
        Debug.Log($"<color=#9900FF>Rally point has been set to {_rallyPoint}</color>");
    }

    public void ResetRallyPoint()
    {
        _isSet = false;
        Debug.Log($"<color=#FF0099>Rally point has been removed</color>");
    }
}
