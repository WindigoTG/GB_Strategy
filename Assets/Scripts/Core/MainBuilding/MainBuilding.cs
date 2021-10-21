using UnityEngine;
using UniRx;

public class MainBuilding : MonoBehaviour, ISelectable, IAttackable, IHealthHolder
{
	[SerializeField] private float _maxHealth = 1000;
	[SerializeField] private Sprite _icon;

    private ReactiveProperty<float> _health = new ReactiveProperty<float>();

	private void Start()
    {
		_health.Value = _maxHealth;
	}

    public void TakeDamage(int amount)
    {
        if (_health.Value <= 0)
        {
            return;
        }

        _health.Value -= amount;

        if (_health.Value <= 0)
        {
            Destroy(gameObject);
        }
    }

    public float CurrentHealth => _health.Value;
	public float MaxHealth => _maxHealth;
	public Sprite Icon => _icon;
	public Vector3 CurrentPosition => transform.position;
    public ReactiveProperty<float> ObservableHealth => _health;
}

