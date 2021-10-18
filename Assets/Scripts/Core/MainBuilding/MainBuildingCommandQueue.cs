using UnityEngine;
using Zenject;

public class MainBuildingCommandQueue : MonoBehaviour, ICommandsQueue
{
	[Inject] CommandExecutorBase<IProduceUnitCommand> _produceUnitCommandExecutor;
	[Inject] CommandExecutorBase<ISetRallyPointCommand> _setRallyPointCommandExecutor;
	[Inject] CommandExecutorBase<IRemoveRallyPointCommand> _removeRallyPointCommandExecutor;

	public void Clear() { }

	public async void EnqueueCommand(object command)
	{
		await _produceUnitCommandExecutor.TryExecuteCommand(command);
		await _setRallyPointCommandExecutor.TryExecuteCommand(command);
		await _removeRallyPointCommandExecutor.TryExecuteCommand(command);
	}

	public ICommand CurrentCommand => default;
}
