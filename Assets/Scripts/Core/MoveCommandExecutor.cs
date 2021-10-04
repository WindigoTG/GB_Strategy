using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator), typeof(UnitMovementStop), typeof(StopCommandExecutor))]
public class MoveCommandExecutor : CommandExecutorBase<IMoveCommand>
{
    private UnitMovementStop _stop;
    private Animator _animator;
    private StopCommandExecutor _stopExecutor;
    private NavMeshAgent _navMeshAgent;
    private NavMeshObstacle _navMeshObstacle;

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
        Debug.Log($"<color=#009900>{name} is moving to {command.Target}</color>");

        try
        {
            MakeMovable(true);

            _navMeshAgent.destination = command.Target;
            _animator.SetTrigger("Walk");
            await _stop.WithCancellation(_stopExecutor.CToken);
        }
        catch
        {
            _navMeshAgent.isStopped = true;
            _navMeshAgent.ResetPath();
        }

        _animator.SetTrigger("Idle");

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
