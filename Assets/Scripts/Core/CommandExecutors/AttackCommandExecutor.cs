using System;
using System.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

public partial class AttackCommandExecutor : CommandExecutorBase<IAttackCommand>
{
	private Animator _animator;
	private StopCommandExecutor _stopCommandExecutor;
	private IHoldPositionExecutor _holdPositionExecutor;
	private IAutomaticAttacker _automaticAttacker;
	private NavMeshAgent _navMeshAgent;
	private UnitNavigationStateManager _navigationState;

	[Inject] private IHealthHolder _ourHealth;
	[Inject(Id = "AttackDistance")] private float _attackingDistance;
	[Inject(Id = "AttackPeriod")] private int _attackingPeriod;

	private Vector3 _ourPosition;
	private Vector3 _targetPosition;
	private Quaternion _ourRotation;

	private readonly Subject<Vector3> _targetPositions = new Subject<Vector3>();
	private readonly Subject<Quaternion> _targetRotations = new Subject<Quaternion>();
	private readonly Subject<IAttackable> _attackTargets = new Subject<IAttackable>();

	private Transform _targetTransform;
	private AttackOperation _currentAttackOp;

	[Inject]
	private void Init()
	{
		_targetPositions
			.Select(value => new Vector3((float)Math.Round(value.x, 2), (float)Math.Round(value.y, 2), (float)Math.Round(value.z, 2)))
			.Distinct()
			.ObserveOnMainThread()
			.Subscribe(StartMovingToPosition);

		_attackTargets
			.ObserveOnMainThread()
			.Subscribe(StartAttackingTargets);

		_targetRotations
			.ObserveOnMainThread()
			.Subscribe(SetAttackRoation);

		_animator = GetComponent<Animator>();
		_stopCommandExecutor = GetComponent<StopCommandExecutor>();
		_navMeshAgent = GetComponent<NavMeshAgent>();
		_navigationState = GetComponent<UnitNavigationStateManager>();
		TryGetComponent<IHoldPositionExecutor>(out _holdPositionExecutor);
		TryGetComponent<IAutomaticAttacker>(out _automaticAttacker);
	}

	private void StartMovingToPosition(Vector3 position)
	{
		if (!_navMeshAgent.hasPath)
		{
			_navigationState.MakeUnitMovable(true);
			_animator.SetTrigger("Walk");
		}

		_navMeshAgent.destination = position;
	}

	private void StartAttackingTargets(IAttackable target)
	{
		if (_navMeshAgent.isActiveAndEnabled)
		{
			_navMeshAgent.isStopped = true;
			_navMeshAgent.ResetPath();
		}
		_animator.SetTrigger("Attack");
		target.TakeDamage(GetComponent<IDamageDealer>().Damage);
	}

	private void SetAttackRoation(Quaternion targetRotation)
	{
		transform.rotation = targetRotation;
	}

	public override async Task ExecuteSpecificCommand(IAttackCommand command)
    {
		if (command.Target != null &&
			(command.Target as Component).gameObject != gameObject)
		{
			Debug.Log($"<color=#FF0000>{name} attacks {command.Target}</color>");

			_targetTransform = (command.Target as Component).transform;
			_currentAttackOp = new AttackOperation(this, command.Target);
			Update();
			try
			{
				await _currentAttackOp.WithCancellation(_stopCommandExecutor.CToken);
			}
			catch
			{
				_currentAttackOp.Cancel();
			}
			_animator.SetTrigger("Idle");
			_navigationState.MakeUnitMovable(false);
			_currentAttackOp = null;
			_targetTransform = null;
		}
	}

	private void Update()
	{
		if (_currentAttackOp == null)
		{
			return;
		}

		lock (this)
		{
			_ourPosition = transform.position;
			_ourRotation = transform.rotation;
			if (_targetTransform != null)
			{
				_targetPosition = _targetTransform.position;
			}
		}
	}
}
