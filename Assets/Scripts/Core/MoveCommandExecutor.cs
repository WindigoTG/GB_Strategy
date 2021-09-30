using UnityEngine;

public class MoveCommandExecutor : CommandExecutorBase<IMoveCommand>
{
    public override void ExecuteSpecificCommand(IMoveCommand command)
    {
        Debug.Log($"<color=#00FF00>{name} is moving to {command.Target}</color>");
    }
}
