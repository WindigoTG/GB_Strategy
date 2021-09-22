using UnityEngine;

public class MainBuilding : MonoBehaviour, IUnitProducer, ISelectable
{
	[SerializeField] private GameObject _unitPrefab;
	[SerializeField] private Transform _unitsParent;

	[SerializeField] private float _maxHealth = 1000;
	[SerializeField] private Sprite _icon;

	private float _health;

    private void Start()
    {
		_health = _maxHealth;
    }

    public void ProduceUnit()
	{
		Instantiate(_unitPrefab, new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10)), Quaternion.identity, _unitsParent);
	}

	public float Health => _health;
	public float MaxHealth => _maxHealth;
	public Sprite Icon => _icon;

}

