using UniRx;

public interface IUnitProducer
{
	IReadOnlyReactiveCollection<IUnitProductionTask> Queue { get; }
	public void Cancel(int index);
}
