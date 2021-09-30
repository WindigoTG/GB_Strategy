using UnityEngine;

public class PatrolCommandExecutor : CommandExecutorBase<IPatrolCommand>
{
    public override void ExecuteSpecificCommand(IPatrolCommand command)
    {
        Debug.Log($"<color=#00FFFF>{name} patrols between {command.StartPosition} and {command.TargetPosition}</color>");
    }
}