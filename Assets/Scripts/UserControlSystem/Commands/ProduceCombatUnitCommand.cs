using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ProduceCombatUnitCommand : IProduceCombatUnitCommand
{
	[Inject(Id = "Chomper")] public string UnitName { get; }
	[Inject(Id = "Chomper")] public Sprite Icon { get; }
	[Inject(Id = "Chomper")] public float ProductionTime { get; }

	[InjectAsset("Chomper")] protected GameObject _unitPrefab;

	public GameObject UnitPrefab => _unitPrefab;

	[Inject(Id = "Chomper")] Dictionary<ResourceType, int> _resourceCost;
	public IReadOnlyDictionary<ResourceType, int> ResourceCost => _resourceCost;
}
