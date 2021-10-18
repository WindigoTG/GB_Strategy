using UnityEngine;
using Zenject;
using System.Collections.Generic;

[RequireComponent(typeof(Outline))]
public class FactionMember : MonoBehaviour, IFactionMember
{
	[SerializeField][Range(0,6)] private int _factionId;

	private Outline _outline;
	private Color[] Colors =
		{
			Color.white,
			Color.green,
			Color.red,
			Color.blue,
			Color.cyan,
			Color.magenta,
			Color.yellow,
		};

	[Inject]
	private readonly Dictionary<int, int> _factionMemberCounter;

    private void Awake()
    {
		_outline = GetComponent<Outline>();
	}

    private void Start()
    {
		_outline.OutlineColor = Colors[_factionId];
		_outline.enabled = false;

		if (_factionId != 0)
			AddMemberToCount();
    }

    public void SetFaction(int factionId)
	{
		_factionId = factionId;
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
