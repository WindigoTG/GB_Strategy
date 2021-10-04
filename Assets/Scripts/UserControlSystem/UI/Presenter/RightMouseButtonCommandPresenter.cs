using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class RightMouseButtonCommandPresenter : MonoBehaviour
{
    [Inject] private SelectableValue _selectable;
    [Inject] private AttackTargetValue _attackTargetObject;
    [Inject] private Vector3Value _groundClicksRMB;

    [Inject] private CommandCreatorBase<IMoveCommand> _mover;
    [Inject] private CommandCreatorBase<IAttackCommand> _attacker;

    private ISelectable _currentSelectable;

    private bool _isMoveCommandPending;
    private bool _isAttackCommandPending;

    void Start()
    {
        _selectable.OnNewValue += OnSelected;
        OnSelected(_selectable.CurrentValue);
    }

    void Update()
    {
        if (_currentSelectable != null)
        {
            if (!_isAttackCommandPending)
                InitiateAttackCommandCreationProcess();

            if (!_isMoveCommandPending)
                InitiateMoveCommandCreationProcess();
        }
    }

    private void OnSelected(ISelectable selectable)
    {
        if (_currentSelectable == selectable)
        {
            return;
        }

        ProcessOnCancel();

        _currentSelectable = selectable;
    }

    private void InitiateMoveCommandCreationProcess()
    {
        _isMoveCommandPending = true;

        var moveExecutor = (_currentSelectable as Component).GetComponentInParent<CommandExecutorBase<IMoveCommand>>();
        if (moveExecutor != null)
        {
            _mover.ProcessCommandExecutor(moveExecutor, command => ExecuteCommandWrapper(moveExecutor, command));
        }
    }

    private void InitiateAttackCommandCreationProcess()
    {
        _isAttackCommandPending = true;

        var attackExecutor = (_currentSelectable as Component).GetComponentInParent<CommandExecutorBase<IAttackCommand>>();
        if (attackExecutor != null)
        {
            _attacker.ProcessCommandExecutor(attackExecutor, command => ExecuteCommandWrapper(attackExecutor, command));
        }
    }

    public void ExecuteCommandWrapper(ICommandExecutor commandExecutor, object command)
    {
        commandExecutor.ExecuteCommand(command);

        if (commandExecutor is CommandExecutorBase<IMoveCommand>)
            _isMoveCommandPending = false;
        else
            _isAttackCommandPending = false;
    }

    private void ProcessOnCancel()
    {
        _attacker.ProcessCancel();
        _mover.ProcessCancel();
        _isMoveCommandPending = false;
        _isAttackCommandPending = false;
    }
}
