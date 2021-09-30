using System;
using Zenject;

public class StopCommandCreator : CommandCreatorBase<IStopCommand>
{
	[Inject] private AssetsContext _context;

	protected override void ClassSpecificCommandCreation(Action<IStopCommand> creationCallback)
	{
		creationCallback?.Invoke(_context.Inject(new StopCommand()));
	}
}
