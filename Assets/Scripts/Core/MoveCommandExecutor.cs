using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator), typeof(UnitMovementStop), typeof(StopCommandExecutor))]
public class MoveCommandExecutor : CommandExecutorBase<IMoveCommand>
{
    private UnitMovementStop _stop;
    private Animator _animator;
    private StopCommandExecutor _stopExecutor;
    private NavMeshAgent _navMeshAgent;

    void Awake()
    {
        _stop = GetComponent<UnitMovementStop>();
        _animator = GetComponent<Animator>();
        _stopExecutor = GetComponent<StopCommandExecutor>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    public override async void ExecuteSpecificCommand(IMoveCommand command)
    {
        Debug.Log($"<color=#00FF00>{name} is moving to {command.Target}</color>");

        try
        {
            _navMeshAgent.destination = command.Target;
            _animator.SetTrigger("Walk");
            await _stop.WithCancellation(_stopExecutor.CToken);
        }
        catch
        {
            GetComponent<NavMeshAgent>().isStopped = true;
            _navMeshAgent.ResetPath();
        }

        _animator.SetTrigger("Idle");
    }
}
