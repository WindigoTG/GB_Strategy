public class GatherResourceCommandCreator : CancellableCommandCreatorBase<IGatherResourceCommand, IGatherable>
{
    protected override IGatherResourceCommand CreateCommand(IGatherable argument) => new GatherResourceCommand(argument);
}
