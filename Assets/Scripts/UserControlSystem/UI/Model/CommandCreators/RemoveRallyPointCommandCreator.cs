using System;
using Zenject;

public class RemoveRallyPointCommandCreator : CommandCreatorBase<IRemoveRallyPointCommand>
{
	[Inject] private AssetsContext _context;

	protected override void ClassSpecificCommandCreation(Action<IRemoveRallyPointCommand> creationCallback)
	{
		creationCallback?.Invoke(_context.Inject(new RemoveRallyPointCommand()));
	}
}
