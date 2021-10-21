using UnityEngine;
using UniRx;
using Zenject;

public class CombatUnitCommandsQueue : MonoBehaviour, ICommandsQueue
{
    [Inject] CommandExecutorBase<IMoveCommand> _moveCommandExecutor;
    [Inject] CommandExecutorBase<IPatrolCommand> _patrolCommandExecutor;
    [Inject] CommandExecutorBase<IAttackCommand> _attackCommandExecutor;
    [Inject] CommandExecutorBase<IStopCommand> _stopCommandExecutor;
    [Inject] CommandExecutorBase<IHoldPositionCommand> _holdPositionCommandExecutor;

	private ReactiveCollection<ICommand> _commandsAwaitingExecution = new ReactiveCollection<ICommand>();

	private IHoldPositionExecutor _holdPositionExecutor;

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
		await _patrolCommandExecutor?.TryExecuteCommand(command);
		await _attackCommandExecutor?.TryExecuteCommand(command);
		await _stopCommandExecutor?.TryExecuteCommand(command);
		await _holdPositionCommandExecutor?.TryExecuteCommand(command);

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

		if (!(command is IAutoAttackCommand))
			_holdPositionExecutor?.CancelHoldPosition();

		_commandsAwaitingExecution.Add(command);
	}

	public void Clear()
	{
		_commandsAwaitingExecution.Clear();
		_stopCommandExecutor.ExecuteSpecificCommand(new StopCommand());
	}

	public ICommand CurrentCommand => _commandsAwaitingExecution.Count > 0 ? _commandsAwaitingExecution[0] : default;
}
