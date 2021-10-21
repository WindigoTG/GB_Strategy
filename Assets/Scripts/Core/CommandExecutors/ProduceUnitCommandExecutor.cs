using UnityEngine;
using UniRx;
using System.Threading.Tasks;

[RequireComponent(typeof(UnitProdactionQueue))]
public abstract class ProduceUnitCommandExecutor<T> : CommandExecutorBase<T>, IUnitProducer where T : class, IProduceUnitCommand
{
	private UnitProdactionQueue _productionQueue;

	public IReadOnlyReactiveCollection<IUnitProductionTask> Queue => _productionQueue.Queue;

	private void Awake()
	{
		_productionQueue = GetComponent<UnitProdactionQueue>();
	}

	public void Cancel(int index)
	{
		_productionQueue.Cancel(index);
	}

	public override async Task ExecuteSpecificCommand(T command)
	{
		_productionQueue.Enqueue(new UnitProductionTask(command.ProductionTime, command.Icon, command.UnitPrefab, command.UnitName, command.ResourceCost));
		await Task.CompletedTask;
	}
}
