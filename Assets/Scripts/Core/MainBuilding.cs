using System.Threading.Tasks;
using UnityEngine;
using UniRx;
using System;
using Random = UnityEngine.Random;
using System.Collections;

public class MainBuilding : CommandExecutorBase<IProduceUnitCommand>, ISelectable, IAttackTarget
{
	[SerializeField] private Transform _unitsParent;

	[SerializeField] private float _maxHealth = 1000;
	[SerializeField] private Sprite _icon;

	private float _health;

	private int _unitBuildTime = 5;
	private bool _isConstructing;

	private ReactiveProperty<int> _buildTime = new ReactiveProperty<int>();

	private void Start()
    {
		_health = _maxHealth;
	}

	public override void ExecuteSpecificCommand(IProduceUnitCommand command)
	{
		if (!_isConstructing)
		{
			Debug.Log($"<color=#FF00FF>{name} has begun {command.UnitPrefab.name} construction</color>");

			Observable.FromCoroutine(CountDown).Subscribe(_ =>
			{
				Instantiate(command.UnitPrefab, new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10)), Quaternion.identity, _unitsParent);
				Debug.Log($"<color=#00FF00>Construction complete</color>");
				_isConstructing = false;
			});
		}
	}

	IEnumerator CountDown()
    {
		_isConstructing = true;
		_buildTime.Value = _unitBuildTime;

		do
		{
			Debug.Log($"<color=#FFFF00>{_buildTime.Value--} seconds remaining</color>");
			yield return Observable.Timer(TimeSpan.FromSeconds(1)).ToYieldInstruction();
		} while (_buildTime.Value > 0);
    }

	public float Health => _health;
	public float MaxHealth => _maxHealth;
	public Sprite Icon => _icon;
	public Vector3 CurrentPosition => transform.position;
}

