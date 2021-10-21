using System;

public class ProduceCombatUnitCommandCreator : ProduceUnitCommandCreator<IProduceCombatUnitCommand>
{
	
    protected override void ClassSpecificCommandCreation(Action<IProduceCombatUnitCommand> creationCallback)
    {
		var produceUnitCommand = _context.Inject(new ProduceCombatUnitCommand());
		_diContainer.Inject(produceUnitCommand);
		creationCallback?.Invoke(produceUnitCommand);
	}
}
