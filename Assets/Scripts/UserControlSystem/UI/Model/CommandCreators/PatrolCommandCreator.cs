using UnityEngine;
using Zenject;

public class PatrolCommandCreator : CancellableCommandCreatorBase<IPatrolCommand, Vector3>
{
	[Inject] private SelectableValue _selectable;

	protected override IPatrolCommand CreateCommand(Vector3 argument) => new PatrolCommand(_selectable.CurrentValue.CurrentPosition, argument);
}
