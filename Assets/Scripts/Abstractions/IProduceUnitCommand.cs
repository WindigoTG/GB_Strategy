using System.Collections.Generic;
using UnityEngine;

public interface IProduceUnitCommand : ICommand, IIconHolder
{
	float ProductionTime { get; }
	GameObject UnitPrefab { get; }
	string UnitName { get; }
	IReadOnlyDictionary<ResourceType, int> ResourceCost { get; }
}

