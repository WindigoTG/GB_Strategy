using UnityEngine;

public class GatherableResource : MonoBehaviour, IGatherable, ISelectable
{
    [SerializeField] private ResourceType _resourceType;
    [SerializeField] private Sprite _icon;

    public ResourceType ResourceType => _resourceType;

    public Vector3 CurrentPosition => transform.position;

    public Sprite Icon => _icon;
}
