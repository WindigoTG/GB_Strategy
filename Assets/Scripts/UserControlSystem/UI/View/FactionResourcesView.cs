using UnityEngine;
using TMPro;
using System.Collections.Generic;
using Zenject;
using UniRx;

public class FactionResourcesView : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _factionNameText;

    Dictionary<ResourceType, ResourceAmountView> _resourcesViews = new Dictionary<ResourceType, ResourceAmountView>();

    [Inject(Id = "ResourceView")] GameObject _resorceViewPrefab;

    [Inject] DiContainer _diContainer;

    [Inject] ResourceIconByType _resourceIconContainer;

    public void Init(string factionID, ReactiveDictionary<ResourceType, int> resourceData)
    {
        _factionNameText.text = $"Faction {factionID}";

        foreach (var resource in resourceData)
        {
            var resourceView = _diContainer.InstantiatePrefab(_resorceViewPrefab, transform).GetComponent<ResourceAmountView>();
            resourceView.ResouurceAmountText.text = resource.Value.ToString();

            var icon = _resourceIconContainer.GetIconByType(resource.Key);
            if (icon != null)
                resourceView.ResourceIcon.sprite = icon;

            _resourcesViews.Add(resource.Key, resourceView);
        }

        resourceData.ObserveReplace().Subscribe(UpdateResourceCount);
    }

    private void UpdateResourceCount(DictionaryReplaceEvent<ResourceType, int> obj)
    {
        _resourcesViews[obj.Key].ResouurceAmountText.text = obj.NewValue.ToString();
    }
}
