using UnityEngine;

public class UnitBase : MonoBehaviour, ISelectable, IAttackTarget
{
    [SerializeField] private float _maxHealth = 100;
    [SerializeField] private Sprite _icon;

    private float _health;

    private void Start()
    {
        _health = _maxHealth;
    }

    public float Health => _health;
    public float MaxHealth => _maxHealth;
    public Sprite Icon => _icon;
    public Vector3 CurrentPosition => transform.position;
}
