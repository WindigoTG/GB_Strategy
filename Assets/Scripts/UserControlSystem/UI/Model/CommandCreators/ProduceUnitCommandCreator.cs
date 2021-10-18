using System;
using Zenject;

public abstract class ProduceUnitCommandCreator<T> : CommandCreatorBase<T> where T : class, IProduceUnitCommand
{
	[Inject] protected AssetsContext _context;
	[Inject] protected DiContainer _diContainer;
}
