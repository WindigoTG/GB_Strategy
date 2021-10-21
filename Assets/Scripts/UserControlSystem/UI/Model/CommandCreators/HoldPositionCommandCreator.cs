using System;

public class HoldPositionCommandCreator : CommandCreatorBase<IHoldPositionCommand>
{
    protected override void ClassSpecificCommandCreation(Action<IHoldPositionCommand> creationCallback)
    {
        creationCallback.Invoke(new HoldPositionCommand());
    }
}
