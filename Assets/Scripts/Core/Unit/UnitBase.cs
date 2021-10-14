using UnityEngine;
using UniRx;

public class UnitBase : MonoBehaviour, ISelectable, IAttackable, IDamageDealer
{
    [SerializeField] private float _maxHealth = 100;
    [SerializeField] private Sprite _icon;
    [SerializeField] private int _damage = 25;

    private Animator _animator;
    private StopCommandExecutor _stopCommand;

    private ReactiveProperty<float> _health = new ReactiveProperty<float>();

    private void Start()
    {
        _health.Value = _maxHealth;
        _animator = GetComponent<Animator>();
        _stopCommand = GetComponent<StopCommandExecutor>();
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
            _animator.SetTrigger("Death");
            Invoke(nameof(DestroyUnit), 2f);
        }
    }

    private async void DestroyUnit()
    {
        await _stopCommand.ExecuteSpecificCommand(new StopCommand());
        Destroy(gameObject);
    }

    public float CurrentHealth => _health.Value;
    public float MaxHealth => _maxHealth;
    public Sprite Icon => _icon;
    public Vector3 CurrentPosition => transform.position;
    public int Damage => _damage;
    public ReactiveProperty<float> ObservableHealth => _health;
}
