using UnityEngine;

public class MouseInteractionsPresenter : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private SelectableValue _selectedObject;

    private Outline _outline;

    private void Update()
    {
        if (!Input.GetMouseButtonUp(0))
        {
            return;
        }

        var hits = Physics.RaycastAll(_camera.ScreenPointToRay(Input.mousePosition));

        if (hits.Length == 0)
        {
            return;
        }

        if (_outline != null)
            _outline.enabled = false;

        ISelectable selectable = null;

        foreach (var hit in hits)
        {
            selectable = hit.collider.GetComponentInParent<ISelectable>();

            if (selectable != null)
            {
                _outline = hit.collider.transform.parent.GetOrAddComponent<Outline>();
                _outline.enabled = true;
                break;
            }
            
        }

        _selectedObject.SetValue(selectable);
    }
}

