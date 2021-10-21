using UnityEngine;
using Zenject;
using UniRx;
using System.Collections.Generic;

public class ResourceRecipient : MonoBehaviour, IResourceRecipient
{
    [Inject] Dictionary<int, ReactiveDictionary<ResourceType, int>> _resourcesRepository;
    [Inject] List<IResourceRecipient> _resourceRecipientsRepository;

    private FactionMember _factionMember;

    private void Awake()
    {
        _factionMember = GetComponent<FactionMember>();
        lock (this)
        {
            _resourceRecipientsRepository.Add(this);
        }
    }

    public void DepositResources((ResourceType resourceType, int resourceAmount) resourceLoad)
    {
        lock (this)
        {
            _resourcesRepository[FactionID][resourceLoad.resourceType] += resourceLoad.resourceAmount;
        }
    }

    private void OnDestroy()
    {
        lock (this)
        {
            _resourceRecipientsRepository.Remove(this);
        }
    }

    public int FactionID => _factionMember.FactionId;

}
