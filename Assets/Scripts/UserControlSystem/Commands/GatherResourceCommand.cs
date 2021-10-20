public class GatherResourceCommand : IGatherResourceCommand
{
    private IGatherable _gatherableResource;

    public GatherResourceCommand(IGatherable resource)
    {
        _gatherableResource = resource;
    }

    public IGatherable GatherableResource => _gatherableResource;
}
