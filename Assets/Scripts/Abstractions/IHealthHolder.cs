using UniRx;

public interface IHealthHolder
{
	ReactiveProperty<float> ObservableHealth { get; }
	float CurrentHealth { get; }
	float MaxHealth { get; }
}
