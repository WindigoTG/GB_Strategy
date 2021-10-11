using UnityEngine;

public interface ISelectable : IHealthHolder, IIconHolder
{
	Vector3 CurrentPosition { get; }
}

