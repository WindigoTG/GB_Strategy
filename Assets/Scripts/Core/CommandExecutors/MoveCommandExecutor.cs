using UnityEngine;
using UnityEngine.AI;
using System.Threading.Tasks;
using Zenject;

[RequireComponent(typeof(Animator), typeof(UnitMovementStop), typeof(StopCommandExecutor))]
public class MoveCommandExecutor : CommandExecutorBase<IMoveCommand>
{
    private UnitMovementStop _stop;
    private Animator _animator;
    private StopCommandExecutor _stopExecutor;
    private NavMeshAgent _navMeshAgent;
    private UnitNavigationStateManager _navigationState;

    [Inject]
    void Init()
    {
        _stop = GetComponent<UnitMovementStop>();
        _animator = GetComponent<Animator>();
        _stopExecutor = GetComponent<StopCommandExecutor>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navigationState = GetComponent<UnitNavigationStateManager>();
    }

    public override async Task ExecuteSpecificCommand(IMoveCommand command)
    {
        Debug.Log($"<color=#009900>{name} is moving to {command.Target}</color>");

        try
        {
            _navigationState.MakeUnitMovable(true);

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

        _navigationState.MakeUnitMovable(false);
    }
}
