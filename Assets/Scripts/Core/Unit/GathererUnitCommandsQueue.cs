using UnityEngine;
using UniRx;
using Zenject;

public class GathererUnitCommandsQueue : MonoBehaviour, ICommandsQueue
{
    [Inject] CommandExecutorBase<IMoveCommand> _moveCommandExecutor;
    [Inject] CommandExecutorBase<IStopCommand> _stopCommandExecutor;
	[Inject] CommandExecutorBase<IGatherResourceCommand> _gatherResourceCommandExecutor;

	private ReactiveCollection<ICommand> _commandsAwaitingExecution = new ReactiveCollection<ICommand>();

	[Inject]
	private void Init()
	{
		_commandsAwaitingExecution
			.ObserveAdd().Subscribe(OnNewCommand).AddTo(this);
	}

	private void OnNewCommand(ICommand command, int index)
	{
		if (index == 0)
		{
			ExecuteCommand(command);
		}
	}

	private async void ExecuteCommand(ICommand command)
	{
		await _moveCommandExecutor?.TryExecuteCommand(command);
		await _stopCommandExecutor?.TryExecuteCommand(command);
		_gatherResourceCommandExecutor?.TryExecuteCommand(command);

		if (_commandsAwaitingExecution.Count > 0)
		{
			_commandsAwaitingExecution.RemoveAt(0);
		}
		CheckTheQueue();
	}

	private void CheckTheQueue()
	{
		if (_commandsAwaitingExecution.Count > 0)
		{
			ExecuteCommand(_commandsAwaitingExecution[0]);
		}
	}

	public void EnqueueCommand(object wrappedCommand)
	{
		var command = wrappedCommand as ICommand;

		_commandsAwaitingExecution.Add(command);
	}

	public void Clear()
	{
		_commandsAwaitingExecution.Clear();
		_stopCommandExecutor.ExecuteSpecificCommand(new StopCommand());
	}

	public ICommand CurrentCommand => _commandsAwaitingExecution.Count > 0 ? _commandsAwaitingExecution[0] : default;
}
