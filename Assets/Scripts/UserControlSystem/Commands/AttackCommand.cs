public class AttackCommand : IAttackCommand
{
	public IAttackTarget Target { get; }

    public AttackCommand(IAttackTarget target)
	{
		Target = target;
	}
}
  