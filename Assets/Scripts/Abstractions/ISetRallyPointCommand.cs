using UnityEngine;

public interface ISetRallyPointCommand : ICommand
{
	public Vector3 Target { get; }
}