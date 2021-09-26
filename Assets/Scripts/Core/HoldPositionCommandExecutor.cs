using UnityEngine;

public class HoldPositionCommandExecutor : CommandExecutorBase<IHoldPositionCommand>
{
    public override void ExecuteSpecificCommand(IHoldPositionCommand command)
    {
        Debug.Log("<color=#FFFF00>Hold position</color>");
    }
}
