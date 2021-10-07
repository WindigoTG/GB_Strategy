using UnityEngine;
using Zenject;
using System;
using UniRx;

public class OutlineSelectorPresenter : MonoBehaviour
{
    [Inject] private IObservable<ISelectable> _selectable;

    private Outline _outline;
    private ISelectable _currentSelectable;

    private void Start()
    {
        _selectable.Subscribe(OnSelected);
    }

    private void OnSelected(ISelectable selectable)
    {
        if (_currentSelectable == selectable)
        {
            return;
        }

        if (_outline != null)
            _outline.enabled = false;

        _currentSelectable = selectable;

        if (selectable != null)
        {
            _outline = (selectable as Component).GetOrAddComponent<Outline>();
            _outline.enabled = true;
        }
    }
}
