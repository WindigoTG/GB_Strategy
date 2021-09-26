using UnityEngine;
using UnityEngine.EventSystems;

public class MouseInteractionsPresenter : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private SelectableValue _selectedObject;
    [SerializeField] private EventSystem _eventSystem;

    private void Update()
    {
        if (!Input.GetMouseButtonUp(0))
            return;

        if (_eventSystem.IsPointerOverGameObject())
            return;

        var hits = Physics.RaycastAll(_camera.ScreenPointToRay(Input.mousePosition));

        if (hits.Length == 0)
            return;
        

        ISelectable selectable = null;
        Outline outline = null;

        foreach (var hit in hits)
        {
            selectable = hit.collider.GetComponentInParent<ISelectable>();

            if (selectable != null)
            {
                outline = hit.collider.transform.parent.GetOrAddComponent<Outline>();
                outline.enabled = true;
                break;
            }
            
        }

        _selectedObject.Deselect();
        _selectedObject.SetValue(selectable, outline);
    }
}

