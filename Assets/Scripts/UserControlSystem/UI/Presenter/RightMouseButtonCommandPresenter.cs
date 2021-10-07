using UniRx;
using UnityEngine;
using Zenject;
using System;

public class RightMouseButtonCommandPresenter : MonoBehaviour
{
    [Inject] private IObservable<ISelectable> _selectable;

    [Inject] private CommandCreatorBase<IMoveCommand> _mover;
    [Inject] private CommandCreatorBase<IAttackCommand> _attacker;

    private IReactiveProperty<ISelectable> _currentSelectable = new ReactiveProperty<ISelectable>();

    private IReactiveProperty<bool> _isMoveCommandPending = new ReactiveProperty<bool>(true);
    private IReactiveProperty<bool> _isAttackCommandPending = new ReactiveProperty<bool>(true);

    void Start()
    {
        _selectable.Subscribe(OnSelected);

        _currentSelectable.Where(x => x == null)
            .Subscribe(_ =>
                {
                    _isMoveCommandPending.Value = true;
                    _isAttackCommandPending.Value = true;
                });

        _currentSelectable.Where(x => x != null)
            .Subscribe(_ =>
                {
                    _isMoveCommandPending.Value = false;
                    _isAttackCommandPending.Value = false;
                });

        _isMoveCommandPending.Where(x => x == false)
            .Subscribe(_ => InitiateMoveCommandCreationProcess());

        
        _isAttackCommandPending.Where(x => x == false)
            .Subscribe(_ => InitiateAttackCommandCreationProcess());
    }

    private void OnSelected(ISelectable selectable)
    {
        if (_currentSelectable.Value == selectable)
        {
            return;
        }

        ProcessOnCancel();

        _currentSelectable.Value = selectable;
    }

    private void InitiateMoveCommandCreationProcess()
    {
        _isMoveCommandPending.Value = true;

        var moveExecutor = (_currentSelectable.Value as Component).GetComponentInParent<CommandExecutorBase<IMoveCommand>>();
        if (moveExecutor != null)
        {
            _mover.ProcessCommandExecutor(moveExecutor, command => ExecuteCommandWrapper(moveExecutor, command));
        }
    }

    private void InitiateAttackCommandCreationProcess()
    {
        _isAttackCommandPending.Value = true;

        var attackExecutor = (_currentSelectable.Value as Component).GetComponentInParent<CommandExecutorBase<IAttackCommand>>();
        if (attackExecutor != null)
        {
            _attacker.ProcessCommandExecutor(attackExecutor, command => ExecuteCommandWrapper(attackExecutor, command));
        }
    }

    public void ExecuteCommandWrapper(ICommandExecutor commandExecutor, object command)
    {
        commandExecutor.ExecuteCommand(command);

        if (commandExecutor is CommandExecutorBase<IMoveCommand>)
            _isMoveCommandPending.Value = false;
        else
            _isAttackCommandPending.Value = false;
    }

    private void ProcessOnCancel()
    {
        _attacker.ProcessCancel();
        _mover.ProcessCancel();
        _isMoveCommandPending.Value = true;
        _isAttackCommandPending.Value = true;
    }
}
