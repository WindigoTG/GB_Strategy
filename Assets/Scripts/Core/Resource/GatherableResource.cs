using UnityEngine;

public class GatherableResource : MonoBehaviour, IGatherable
{
    [SerializeField] private ResourceType _resourceType;

    public ResourceType ResourceType => _resourceType;
}
