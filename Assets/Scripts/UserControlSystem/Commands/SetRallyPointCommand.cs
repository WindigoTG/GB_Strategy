using UnityEngine;

public class SetRallyPointCommand : ISetRallyPointCommand
{
	public Vector3 Target { get; }

	public SetRallyPointCommand(Vector3 target)
	{
		Target = target;
	}
}
