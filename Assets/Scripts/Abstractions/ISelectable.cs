using UnityEngine;

public interface ISelectable : IHealthHolder
{
	Sprite Icon { get; }
	Vector3 CurrentPosition { get; }
}

