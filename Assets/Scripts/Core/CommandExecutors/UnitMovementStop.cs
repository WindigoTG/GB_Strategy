using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class UnitMovementStop : MonoBehaviour, IAwaitable<AsyncExtensions.Void>
{
    public class StopAwaiter : AwaiterBase<AsyncExtensions.Void>
    {
        private readonly UnitMovementStop _unitMovementStop;

        public StopAwaiter(UnitMovementStop unitMovementStop)
        {
            _unitMovementStop = unitMovementStop;
            _unitMovementStop.OnStop += OnStop;
        }

        private void OnStop()
        {
            _unitMovementStop.OnStop -= OnStop;
            OnAwaitedEvent(new AsyncExtensions.Void());
        }
    }

    public event Action OnStop;

    private NavMeshAgent _agent;

    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (_agent.isActiveAndEnabled)
        {
            if (!_agent.pathPending)
            {
                if (_agent.remainingDistance <= _agent.stoppingDistance)
                {
                    if (!_agent.hasPath || _agent.velocity.sqrMagnitude == 0f)
                    {
                        OnStop?.Invoke();
                    }
                }
            }
        }
    }

    public IAwaiter<AsyncExtensions.Void> GetAwaiter() => new StopAwaiter(this);
}