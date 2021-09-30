using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class MouseInteractionsPresenter : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private EventSystem _eventSystem;
    [SerializeField] private Transform _groundTransform;

    [Inject] private SelectableValue _selectedObject;
    [Inject] private AttackTargetValue _attackTargetObject;
    [Inject] private Vector3Value _groundClicksRMB;


    private Plane _groundPlane;

    private void Start()
    {
        _groundPlane = new Plane(_groundTransform.up, 0);
    }

    private void Update()
    {
        if (!Input.GetMouseButtonUp(0) && !Input.GetMouseButton(1))
            return;

        if (_eventSystem.IsPointerOverGameObject())
            return;

        if (Input.GetMouseButtonUp(0))
        {
            PerformRaycastToGetValue<ISelectable>(out ISelectable selectable);

            _selectedObject.SetValue(selectable);
        }
        else
        {
            PerformRaycastToGetValue<IAttackTarget>(out IAttackTarget target);

            _attackTargetObject.SetValue(target);

            var ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (_groundPlane.Raycast(ray, out var enter))
            {
                _groundClicksRMB.SetValue(ray.origin + ray.direction * enter);
            }
        }
    }

    private void PerformRaycastToGetValue<T>(out T result)
    {
        result = default;

        var ray = _camera.ScreenPointToRay(Input.mousePosition);
        var hits = Physics.RaycastAll(ray);

        if (hits.Length == 0)
            return;

        foreach (var hit in hits)
        {
            result = hit.collider.GetComponentInParent<T>();

            if (result != null)
                break;
        }
    }
}

