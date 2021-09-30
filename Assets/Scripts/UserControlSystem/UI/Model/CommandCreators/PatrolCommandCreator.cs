using System;
using UnityEngine;
using Zenject;

public class PatrolCommandCreator : CommandCreatorBase<IPatrolCommand>
{
	[Inject] private SelectableValue _selectable;

	private Action<IPatrolCommand> _creationCallback;

	[Inject]
	private void Init(Vector3Value groundClicks)
	{
		groundClicks.OnNewValue += OnNewValue;
	}

	private void OnNewValue(Vector3 groundClick)
	{
		_creationCallback?.Invoke(new PatrolCommand(groundClick, _selectable.CurrentValue.CurrentPosition));
		_creationCallback = null;
	}

	protected override void ClassSpecificCommandCreation(Action<IPatrolCommand> creationCallback)
	{
		_creationCallback = creationCallback;
	}

	public override void ProcessCancel()
	{
		base.ProcessCancel();

		_creationCallback = null;
	}
}
