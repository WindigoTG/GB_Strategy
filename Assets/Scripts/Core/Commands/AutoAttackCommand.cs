public class AutoAttackCommand : IAttackCommand, IAutoAttackCommand
{
	public IAttackable Target { get; }

    public AutoAttackCommand(IAttackable target)
	{
		Target = target;
	}
}
  