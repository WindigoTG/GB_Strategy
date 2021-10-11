using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

[RequireComponent(typeof(Animator), typeof(UnitMovementStop), typeof(StopCommandExecutor))]
public class MoveCommandExecutor : CommandExecutorBase<IMoveCommand>
{
    private UnitMovementStop _stop;
    private Animator _animator;
    private StopCommandExecutor _stopExecutor;
    private NavMeshAgent _navMeshAgent;
    private NavMeshObstacle _navMeshObstacle;

    private Queue<Vector3> _routePoints = new Queue<Vector3>();
    private bool _isMoving;

    void Awake()
    {
        _stop = GetComponent<UnitMovementStop>();
        _animator = GetComponent<Animator>();
        _stopExecutor = GetComponent<StopCommandExecutor>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshObstacle = GetComponent<NavMeshObstacle>();
        
        MakeMovable(false);
    }

    public override async void ExecuteSpecificCommand(IMoveCommand command)
    {
        if (_isMoving && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
        {
            _routePoints.Enqueue(command.Target);
            return;
        }
        else
        {
            _routePoints.Clear();
            _routePoints.Enqueue(command.Target);
        }


        try
        {
            do
            {
                MakeMovable(true);
                var destination = _routePoints.Dequeue();
                Debug.Log($"<color=#009900>{name} is moving to {destination}</color>");
                _navMeshAgent.destination = destination;
                _animator.SetTrigger("Walk");
                _isMoving = true;
                await _stop.WithCancellation(_stopExecutor.CToken);
            } while (_routePoints.Count > 0);
        }
        catch
        {
            _routePoints.Clear();
            _navMeshAgent.isStopped = true;
            _navMeshAgent.ResetPath();
        }

        _animator.SetTrigger("Idle");
        _isMoving = false;

        MakeMovable(false);
    }

    private void MakeMovable(bool isMovable)
    {
        _navMeshAgent.enabled = false;      //Выключать оба элемента перед сменой состояния нет критической необходимости,
        _navMeshObstacle.enabled = false;   //но позволяет избежать предупреждения в консоли,
                                            //в момент когда присвоение конечных значений еще не завершилось.

        _navMeshAgent.enabled = isMovable;
        _navMeshObstacle.enabled = !isMovable;
    }
}
