using System.Threading.Tasks;
using UnityEngine;

public class MainBuilding : CommandExecutorBase<IProduceUnitCommand>, ISelectable, IAttackTarget
{
	[SerializeField] private Transform _unitsParent;

	[SerializeField] private float _maxHealth = 1000;
	[SerializeField] private Sprite _icon;

	private float _health;

	private int _buildTimer;
	private int _unitBuildTime = 5;
	private bool _isConstructing;

    private void Start()
    {
		_health = _maxHealth;
    }

	public override async void ExecuteSpecificCommand(IProduceUnitCommand command)
	{
		if (!_isConstructing)
		{
			_isConstructing = true;
			Debug.Log($"<color=#FF00FF>{name} has begun {command.UnitPrefab.name} construction</color>");

			await BuildCountDown();

			Instantiate(command.UnitPrefab, new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10)), Quaternion.identity, _unitsParent);

			Debug.Log($"<color=#00FF00>Construction complete</color>");
			_isConstructing = false;
		}
	}

	private async Task BuildCountDown()
    {
		_buildTimer = _unitBuildTime;

		do
		{
			Debug.Log($"<color=#FFFF00>{_buildTimer--} seconds remaining</color>");
			await Task.Delay(1000);
		} while (_buildTimer > 0);

	}

	public float Health => _health;
	public float MaxHealth => _maxHealth;
	public Sprite Icon => _icon;
	public Vector3 CurrentPosition => transform.position;
}

