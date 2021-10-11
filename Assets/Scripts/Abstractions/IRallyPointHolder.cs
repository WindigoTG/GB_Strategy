using UnityEngine;

public interface IRallyPointHolder
{
	public Vector3 RallyPoint { get; }
	public bool IsRallyPointSet { get; }
	public void ResetRallyPoint();
}
