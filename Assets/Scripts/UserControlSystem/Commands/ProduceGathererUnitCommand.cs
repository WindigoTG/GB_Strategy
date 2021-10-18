using UnityEngine;
using Zenject;

public class ProduceGathererUnitCommand : IProduceGathererUnitCommand
{
	[Inject(Id = "Gatherer")] public string UnitName { get; }
	[Inject(Id = "Gatherer")] public Sprite Icon { get; }
	[Inject(Id = "Gatherer")] public float ProductionTime { get; }

	[InjectAsset("Gatherer")] protected GameObject _unitPrefab;

	public GameObject UnitPrefab => _unitPrefab;
}
