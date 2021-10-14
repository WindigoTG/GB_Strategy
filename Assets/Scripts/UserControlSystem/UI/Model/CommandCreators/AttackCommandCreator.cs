public class AttackCommandCreator : CancellableCommandCreatorBase<IAttackCommand, IAttackable>
{
	protected override IAttackCommand CreateCommand(IAttackable argument) => new AttackCommand(argument);
}