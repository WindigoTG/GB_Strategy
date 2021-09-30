public interface IAttackCommand : ICommand
{
    public IAttackTarget Target { get; }
}