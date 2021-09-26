using UnityEngine;

public class AttackComandExecutor : CommandExecutorBase<IAttackCommand>
{
    public override void ExecuteSpecificCommand(IAttackCommand command)
    {
        Debug.Log("<color=#FF0000>Attack</color>");
    }
}
