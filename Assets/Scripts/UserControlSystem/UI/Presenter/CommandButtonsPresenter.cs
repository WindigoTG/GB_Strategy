using System;
using System.Collections.Generic;
using UnityEngine;

public class CommandButtonsPresenter : MonoBehaviour
{
    [SerializeField] private SelectableValue _selectable;
    [SerializeField] private CommandButtonsView _view;
    [SerializeField] private AssetsContext _context;

    private ISelectable _currentSelectable;

    private void Start()
    {
        _selectable.OnSelected += onSelected;
        onSelected(_selectable.CurrentValue);

        _view.OnClick += onButtonClick;
    }

    private void onSelected(ISelectable selectable)
    {
        if (_currentSelectable == selectable)
        {
            return;
        }
        _currentSelectable = selectable;

        _view.Clear();
        if (selectable != null)
        {
            var commandExecutors = new List<ICommandExecutor>();
            commandExecutors.AddRange((selectable as Component).GetComponentsInParent<ICommandExecutor>());
            _view.MakeLayout(commandExecutors);
        }
    }

    private void onButtonClick(ICommandExecutor commandExecutor)
    {
        var unitProducer = commandExecutor as CommandExecutorBase<IProduceUnitCommand>;
        if (unitProducer != null)
        {
            unitProducer.ExecuteSpecificCommand(_context.Inject(new ProduceUnitCommandChild()));
            return;
        }

        var attackingUnit = commandExecutor as CommandExecutorBase<IAttackCommand>;
            if (attackingUnit != null)
        {
            attackingUnit.ExecuteSpecificCommand(new AttackCommand());
            return;
        }

        var movingUnit = commandExecutor as CommandExecutorBase<IMoveCommand>;
        if (movingUnit != null)
        {
            movingUnit.ExecuteSpecificCommand(new MoveCommand());
            return;
        }

        var patrolingUnit = commandExecutor as CommandExecutorBase<IPatrolCommand>;
        if (patrolingUnit != null)
        {
            patrolingUnit.ExecuteSpecificCommand(new PatrolCommand());
            return;
        }

        var positionHoldingUnit = commandExecutor as CommandExecutorBase<IHoldPositionCommand>;
        if (positionHoldingUnit != null)
        {
            positionHoldingUnit.ExecuteSpecificCommand(new HoldPositionCommand());
            return;
        }

        var stoppingUnit = commandExecutor as CommandExecutorBase<IStopCommand>;
        if (stoppingUnit != null)
        {
            stoppingUnit.ExecuteSpecificCommand(new StopCommand());
            return;
        }

        throw new ApplicationException($"{nameof(CommandButtonsPresenter)}.{nameof(onButtonClick)}: Unknown type of commands executor: {commandExecutor.GetType().FullName}!");
    }
}
