using System.Threading.Tasks;

public class HoldPositionExecutor : CommandExecutorBase<IHoldPositionCommand>, IHoldPositionExecutor
{
    private bool _isHoldingPosition;

    public void CancelHoldPosition()
    {
        _isHoldingPosition = false;
    }

    public override async Task ExecuteSpecificCommand(IHoldPositionCommand command)
    {
        _isHoldingPosition = true;
        await Task.CompletedTask;
    }

    public bool IsHoldingPosition => _isHoldingPosition;
}
