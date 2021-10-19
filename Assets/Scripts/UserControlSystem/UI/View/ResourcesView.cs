using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Zenject;

public class ResourcesView : MonoBehaviour
{
    Dictionary<int, FactionResourcesView> _factionResourcesViews = new Dictionary<int, FactionResourcesView>();

    [Inject(Id = "FactionResources")] GameObject _factionResorcesViewPrefab;
    [Inject] DiContainer _diContainer;
    [Inject] Dictionary<int, ReactiveDictionary<ResourceType, int>> _resourcesRepository;

    private void Start()
    {
        foreach (var kvp in _resourcesRepository)
        {
            if (!_factionResourcesViews.ContainsKey(kvp.Key))
            {
                var factionResourcesView = _diContainer.InstantiatePrefab(_factionResorcesViewPrefab, transform).GetComponent<FactionResourcesView>();
                factionResourcesView.Init(kvp.Key.ToString(), kvp.Value);
                
                _factionResourcesViews.Add(kvp.Key, factionResourcesView);
            }
        }
    }

}
