using UnityEngine;
using UnityEngine.AI;
using Zenject;

[RequireComponent(typeof(NavMeshAgent), typeof(NavMeshObstacle))]
public class UnitNavigationStateManager : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;
    private NavMeshObstacle _navMeshObstacle;

    [Inject]
    void Init()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshObstacle = GetComponent<NavMeshObstacle>();
    }

    public void MakeUnitMovable(bool isMovable)
    {
        _navMeshAgent.enabled = false;
        _navMeshObstacle.enabled = false;

        _navMeshAgent.enabled = isMovable;
        _navMeshObstacle.enabled = !isMovable;
    }
}
