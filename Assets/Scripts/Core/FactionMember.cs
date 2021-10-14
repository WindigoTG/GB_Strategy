using UnityEngine;
using Zenject;
using System.Collections.Generic;

public class FactionMember : MonoBehaviour, IFactionMember
{
	[SerializeField] private int _factionId;

	[Inject]
	private readonly Dictionary<int, int> _factionMemberCounter;

    private void Start()
    {
		if (_factionId != 0)
			AddMemberToCount();
    }

    public void SetFaction(int factionId)
	{
		_factionId = factionId;
		AddMemberToCount();
	}

	private void AddMemberToCount()
    {
		lock (this)
		{
			if (_factionMemberCounter.ContainsKey(_factionId))
				_factionMemberCounter[_factionId]++;
			else
				_factionMemberCounter.Add(_factionId, 1);
		}
    }

	private void RemoveMemberFromCount()
    {
		lock (this)
		{
			if (_factionMemberCounter.ContainsKey(_factionId))
			{
				_factionMemberCounter[_factionId]--;

				if (_factionMemberCounter[_factionId] <= 0)
					_factionMemberCounter.Remove(_factionId);
			}
		}
	}

	private void OnDestroy()
	{
		RemoveMemberFromCount();
	}

	public int FactionId => _factionId;
}
