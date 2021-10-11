using UnityEngine;

public class AttackComandExecutor : CommandExecutorBase<IAttackCommand>
{
    public override void ExecuteSpecificCommand(IAttackCommand command)
    {
        if (command.Target != null)
            Debug.Log($"<color=#FF0000>{name} attacks {command.Target}</color>");
    }
}
