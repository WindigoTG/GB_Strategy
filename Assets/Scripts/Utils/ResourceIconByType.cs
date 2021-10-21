using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ResourceIconByTypeContainer", menuName = "ResourceIconByTypeContainer")]
public class ResourceIconByType : ScriptableObject
{
    [SerializeField] List<ResourceType> _resourceTypes;
    [SerializeField] List<Sprite> _icons;

    public Sprite GetIconByType(ResourceType type)
    {
        Sprite icon = null;

        if (_resourceTypes.Contains(type))
        {
            int index = _resourceTypes.IndexOf(type);

            if (_icons.Count >= index + 1)
                icon = _icons[index];
        }

        return icon;
    }
}
