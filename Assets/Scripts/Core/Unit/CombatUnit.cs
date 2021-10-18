using UnityEngine;

public class CombatUnit : UnitBase, IAutomaticAttacker
{
    [SerializeField] private float _visionRadius = 8f;
    public float VisionRadius => _visionRadius;
}
