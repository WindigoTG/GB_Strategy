using UnityEngine;

public class CombatUnit : UnitBase, IAutomaticAttacker, IDamageDealer
{
    [SerializeField] private float _visionRadius = 8f;

    [SerializeField] private int _damage = 25;
    public float VisionRadius => _visionRadius;

    public int Damage => _damage;
}
