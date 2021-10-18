public interface IHoldPositionExecutor
{
    bool IsHoldingPosition { get; }
    void CancelHoldPosition();
}
