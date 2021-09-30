using System;
using Zenject;

public class AttackCommandCreator : CommandCreatorBase<IAttackCommand>
{
	private Action<IAttackCommand> _creationCallback;

	[Inject]
	private void Init(AttackTargetValue attackTarget)
	{
		attackTarget.OnNewValue += OnNewValue;
	}

	private void OnNewValue(IAttackTarget target)
	{
		_creationCallback?.Invoke(new AttackCommand(target));
		_creationCallback = null;
	}

	protected override void ClassSpecificCommandCreation(Action<IAttackCommand> creationCallback)
	{
		_creationCallback = creationCallback;
	}

	public override void ProcessCancel()
	{
		base.ProcessCancel();

		_creationCallback = null;
	}
}