public interface IGatherResourceCommand : ICommand
{
    IGatherable GatherableResource { get; }
}
