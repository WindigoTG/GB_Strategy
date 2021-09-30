using UnityEngine;

public interface ISelectable
{
	float Health { get; }
	float MaxHealth { get; }
	Sprite Icon { get; }
	Vector3 CurrentPosition { get; }
}

