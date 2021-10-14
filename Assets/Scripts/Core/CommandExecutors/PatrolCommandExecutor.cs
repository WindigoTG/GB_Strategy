using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using UniRx;
using Zenject;

public partial class PatrolCommandExecutor : CommandExecutorBase<IPatrolCommand>
{
    private Animator _animator;
    private StopCommandExecutor _stopCommandExecutor;
    private NavMeshAgent _navMeshAgent;
    private UnitNavigationStateManager _navigationState;

    private readonly Subject<Vector3> _targetPositions = new Subject<Vector3>();

    private PatrolOperation _currentPatrolOp;
    private Queue<Vector3> _patrolPoints = new Queue<Vector3>();
    private bool _hasReachedDestination;

    [Inject]
    private void Init()
    {
        _targetPositions.ObserveOnMainThread().Subscribe(StartMovingToPosition);

        _animator = GetComponent<Animator>();
        _stopCommandExecutor = GetComponent<StopCommandExecutor>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navigationState = GetComponent<UnitNavigationStateManager>();
    }

    private void StartMovingToPosition(Vector3 position)
    {
        _navMeshAgent.destination = position;
    }

    public override async Task ExecuteSpecificCommand(IPatrolCommand command)
    {
        Debug.Log($"<color=#00FFFF>{name} patrols between {command.StartPosition} and {command.TargetPosition}</color>");

        _patrolPoints.Enqueue(command.TargetPosition);
        _patrolPoints.Enqueue(command.StartPosition);

        _navigationState.MakeUnitMovable(true);
        _animator.SetTrigger("Walk");

        _currentPatrolOp = new PatrolOperation(this);

        try
        {
            await _currentPatrolOp.WithCancellation(_stopCommandExecutor.CToken);
        }
        catch
        {
            _currentPatrolOp.Cancel();
        }
        _animator.SetTrigger("Idle");
        _navigationState.MakeUnitMovable(false);
        _patrolPoints.Clear();
        _currentPatrolOp = null;
    }

    private void Update()
    {
        lock (this)
        {
            _hasReachedDestination = _navMeshAgent.enabled && _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance;
        }
    }
}