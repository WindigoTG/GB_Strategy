using System;
using UnityEngine;
using Zenject;

public class CommandButtonsModel
{
	public event Action<ICommandExecutor> OnCommandAccepted;
	public event Action OnCommandSent;
	public event Action OnCommandCancel;

	[Inject] private CommandCreatorBase<IProduceUnitCommand> _unitProducer;
	[Inject] private CommandCreatorBase<IAttackCommand> _attacker;
	[Inject] private CommandCreatorBase<IStopCommand> _stopper;
	[Inject] private CommandCreatorBase<IMoveCommand> _mover;
	[Inject] private CommandCreatorBase<IPatrolCommand> _patroller;
	[Inject] private CommandCreatorBase<ISetRallyPointCommand> _rallySetter;
	[Inject] private CommandCreatorBase<IRemoveRallyPointCommand> _rallyRemover;
	[Inject] private CommandCreatorBase<IHoldPositionCommand> _positionHolder;

	private bool _isCommandPending;

	public void OnCommandButtonClicked(ICommandExecutor commandExecutor, ICommandsQueue commandsQueue)
	{
		if (_isCommandPending)
		{
			ProcessOnCancel();
		}
		_isCommandPending = true;
		OnCommandAccepted?.Invoke(commandExecutor);

		_unitProducer.ProcessCommandExecutor(commandExecutor, command => ExecuteCommandWrapper(command, commandsQueue));
		_attacker.ProcessCommandExecutor(commandExecutor, command => ExecuteCommandWrapper(command, commandsQueue));
		_stopper.ProcessCommandExecutor(commandExecutor, command => ExecuteCommandWrapper(command, commandsQueue));
		_mover.ProcessCommandExecutor(commandExecutor, command => ExecuteCommandWrapper(command, commandsQueue));
		_patroller.ProcessCommandExecutor(commandExecutor, command => ExecuteCommandWrapper(command, commandsQueue));
		_rallySetter.ProcessCommandExecutor(commandExecutor, command => ExecuteCommandWrapper(command, commandsQueue));
		_rallyRemover.ProcessCommandExecutor(commandExecutor, command => ExecuteCommandWrapper(command, commandsQueue));
		_positionHolder.ProcessCommandExecutor(commandExecutor, command => ExecuteCommandWrapper(command, commandsQueue));
	}

	public void ExecuteCommandWrapper(object command, ICommandsQueue commandsQueue)
	{
		if ((!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift)) ||
			command is IStopCommand)
		{
			commandsQueue.Clear();
		}
		commandsQueue.EnqueueCommand(command);
		_isCommandPending = false;
		OnCommandSent?.Invoke();
	}

	public void OnSelectionChanged()
	{
		_isCommandPending = false;
		ProcessOnCancel();
	}

	private void ProcessOnCancel()
	{
		_unitProducer.ProcessCancel();
		_attacker.ProcessCancel();
		_stopper.ProcessCancel();
		_mover.ProcessCancel();
		_patroller.ProcessCancel();
		_rallySetter.ProcessCancel();
		_rallyRemover.ProcessCancel();
		_positionHolder.ProcessCancel();

		OnCommandCancel?.Invoke();
	}
}
