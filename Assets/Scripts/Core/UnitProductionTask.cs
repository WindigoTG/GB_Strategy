using System.Collections.Generic;
using UnityEngine;

public class UnitProductionTask : IUnitProductionTask
{
	public Sprite Icon { get; }
	public float TimeLeft { get; set; }
	public float ProductionTime { get; }
	public string UnitName { get; }
	public GameObject UnitPrefab { get; }
	public IReadOnlyDictionary<ResourceType, int> ResourceCost { get; }

	public UnitProductionTask(float time, Sprite icon, GameObject unitPrefab, string unitName, IReadOnlyDictionary<ResourceType, int> resourceCost)
	{
		Icon = icon;
		ProductionTime = time;
		TimeLeft = time;
		UnitPrefab = unitPrefab;
		UnitName = unitName;
		ResourceCost = resourceCost;
	}
}
