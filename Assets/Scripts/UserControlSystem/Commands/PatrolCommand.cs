

using UnityEngine;

public class PatrolCommand : IPatrolCommand
{
	public Vector3 TargetPosition { get; }
	public Vector3 StartPosition { get; }

	public PatrolCommand(Vector3 target, Vector3 start)
	{
		TargetPosition = target;
		StartPosition = start;
	}
}