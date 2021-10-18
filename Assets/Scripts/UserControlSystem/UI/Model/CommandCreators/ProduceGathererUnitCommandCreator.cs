using System;

public class ProduceGathererUnitCommandCreator : ProduceUnitCommandCreator<IProduceGathererUnitCommand>
{

	protected override void ClassSpecificCommandCreation(Action<IProduceGathererUnitCommand> creationCallback)
	{
		var produceUnitCommand = _context.Inject(new ProduceGathererUnitCommand());
		_diContainer.Inject(produceUnitCommand);
		creationCallback?.Invoke(produceUnitCommand);
	}
}
