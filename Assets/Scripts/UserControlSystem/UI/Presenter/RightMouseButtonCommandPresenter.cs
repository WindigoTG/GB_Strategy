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
    private ICommandsQueue _currentQueue;

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
                    _currentQueue = null;
                });

        _currentSelectable.Where(x => x != null)
            .Subscribe(_ =>
                {
                    _isMoveCommandPending.Value = false;
                    _isAttackCommandPending.Value = false;

                    var queue = (_currentSelectable.Value as Component).GetComponentInParent<ICommandsQueue>();
                    if (queue != null)
                        _currentQueue = queue;
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

        var moveExecutor = (_currentSelectable.Value as Component).GetComponentInParent<ICommandExecutor<IMoveCommand>>();
        if (moveExecutor != null)
        {
            _mover.ProcessCommandExecutor(moveExecutor, command => ExecuteCommandWrapper(moveExecutor, command));
        }
    }

    private void InitiateAttackCommandCreationProcess()
    {
        _isAttackCommandPending.Value = true;

        var attackExecutor = (_currentSelectable.Value as Component).GetComponentInParent<ICommandExecutor<IAttackCommand>>();
        if (attackExecutor != null)
        {
            _attacker.ProcessCommandExecutor(attackExecutor, command => ExecuteCommandWrapper(attackExecutor, command));
        }
    }

    public void ExecuteCommandWrapper(ICommandExecutor commandExecutor, object command)
    {
        if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift))
        {
            _currentQueue.Clear();
        }
        _currentQueue.EnqueueCommand(command);

        if (commandExecutor is ICommandExecutor<IMoveCommand>)
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
