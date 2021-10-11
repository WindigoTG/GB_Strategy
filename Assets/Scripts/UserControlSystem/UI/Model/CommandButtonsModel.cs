using System;
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

	private bool _isCommandPending;

	public void OnCommandButtonClicked(ICommandExecutor commandExecutor)
	{
		if (_isCommandPending)
		{
			ProcessOnCancel();
		}
		_isCommandPending = true;
		OnCommandAccepted?.Invoke(commandExecutor);

		_unitProducer.ProcessCommandExecutor(commandExecutor, command => ExecuteCommandWrapper(commandExecutor, command));
		_attacker.ProcessCommandExecutor(commandExecutor, command => ExecuteCommandWrapper(commandExecutor, command));
		_stopper.ProcessCommandExecutor(commandExecutor, command => ExecuteCommandWrapper(commandExecutor, command));
		_mover.ProcessCommandExecutor(commandExecutor, command => ExecuteCommandWrapper(commandExecutor, command));
		_patroller.ProcessCommandExecutor(commandExecutor, command => ExecuteCommandWrapper(commandExecutor, command));
		_rallySetter.ProcessCommandExecutor(commandExecutor, command => ExecuteCommandWrapper(commandExecutor, command));
		_rallyRemover.ProcessCommandExecutor(commandExecutor, command => ExecuteCommandWrapper(commandExecutor, command));
	}

	public void ExecuteCommandWrapper(ICommandExecutor commandExecutor, object command)
	{
		commandExecutor.ExecuteCommand(command);
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

		OnCommandCancel?.Invoke();
	}
}
