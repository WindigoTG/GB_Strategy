using UnityEngine;
using Zenject;

public class OutlineSelectorPresenter : MonoBehaviour
{
    [Inject] private SelectableValue _selectable;

    private Outline _outline;
    private ISelectable _currentSelectable;

    private void Start()
    {
        _selectable.OnNewValue += OnSelected;
        OnSelected(_selectable.CurrentValue);
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
