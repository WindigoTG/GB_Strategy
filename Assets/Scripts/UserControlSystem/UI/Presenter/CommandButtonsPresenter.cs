using Zenject;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class CommandButtonsPresenter : MonoBehaviour
{
    [SerializeField] private CommandButtonsView _view;

    [Inject] private IObservable<ISelectable> _selectable;
    [Inject] private CommandButtonsModel _model;

    private ReactiveProperty<ISelectable> _currentSelectable = new ReactiveProperty<ISelectable>();

    private void Start()
    {
        _view.OnClick += _model.OnCommandButtonClicked;
        _model.OnCommandSent += _view.UnblockAllInteractions;
        _model.OnCommandCancel += _view.UnblockAllInteractions;
        _model.OnCommandAccepted += _view.BlockInteractions;

        _selectable.Subscribe(OnSelected);

        _currentSelectable.Where(x => x != null).Subscribe(_ => _model.OnSelectionChanged());
    }

    private void OnSelected(ISelectable selectable)
    {
        if (_currentSelectable.Value == selectable)
        {
            return;
        }

        _currentSelectable.Value = selectable;

        _view.Clear();

        if (selectable != null)
        {
            var commandExecutors = new List<ICommandExecutor>();
            commandExecutors.AddRange((selectable as Component).GetComponentsInParent<ICommandExecutor>());
            var queue = (selectable as Component).GetComponentInParent<ICommandsQueue>();
            _view.MakeLayout(commandExecutors, queue);
        }
    }
}
