using UnityEngine;

public class ProduceUnitCommand : IProduceUnitCommand
{
	[InjectAsset("Chomper")] protected GameObject _unitPrefab;

	public GameObject UnitPrefab => _unitPrefab;
}
