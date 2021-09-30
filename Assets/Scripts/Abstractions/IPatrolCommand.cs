using UnityEngine;

public interface IPatrolCommand : ICommand
{
    public Vector3 TargetPosition { get; }
    public Vector3 StartPosition { get; }
}