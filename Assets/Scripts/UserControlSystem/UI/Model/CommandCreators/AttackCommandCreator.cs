public class AttackCommandCreator : CancellableCommandCreatorBase<IAttackCommand, IAttackTarget>
{
	protected override IAttackCommand CreateCommand(IAttackTarget argument) => new AttackCommand(argument);
}