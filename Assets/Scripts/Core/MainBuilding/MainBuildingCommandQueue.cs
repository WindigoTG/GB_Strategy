using UnityEngine;
using Zenject;

public class MainBuildingCommandQueue : MonoBehaviour, ICommandsQueue
{
	[Inject] ProduceUnitCommandExecutor<IProduceCombatUnitCommand> _produceCombatUnitCommandExecutor;
	[Inject] ProduceUnitCommandExecutor<IProduceGathererUnitCommand> _produceGathererUnitCommandExecutor;
	[Inject] CommandExecutorBase<ISetRallyPointCommand> _setRallyPointCommandExecutor;
	[Inject] CommandExecutorBase<IRemoveRallyPointCommand> _removeRallyPointCommandExecutor;

	public void Clear() { }

	public async void EnqueueCommand(object command)
	{
		await _produceCombatUnitCommandExecutor.TryExecuteCommand(command);
		await _produceGathererUnitCommandExecutor.TryExecuteCommand(command);
		await _setRallyPointCommandExecutor.TryExecuteCommand(command);
		await _removeRallyPointCommandExecutor.TryExecuteCommand(command);
	}

	public ICommand CurrentCommand => default;
}
