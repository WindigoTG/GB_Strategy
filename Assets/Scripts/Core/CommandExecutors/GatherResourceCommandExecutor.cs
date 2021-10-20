using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

public partial class GatherResourceCommandExecutor : CommandExecutorBase<IGatherResourceCommand>
{
    [Inject] List<IResourceRecipient> _resourceRecipientsRepository;

    private (ResourceType type, float amount) _resourceLoad;
    private IResourceRecipient _resourceDropoffPoint;

    private Animator _animator;
    private StopCommandExecutor _stopCommandExecutor;
    private NavMeshAgent _navMeshAgent;
    private UnitNavigationStateManager _navigationState;
    private FactionMember _factionMember;

    [Inject(Id = "InteractionDistance")] private float _interactionDistance;
    [Inject(Id = "GatherSpeed")] private float _gatherSpeed;
    [Inject(Id = "MaximumResourceLoad")] private int _maximumLoad;

    private Vector3 _ourPosition;
    private Quaternion _ourRotation;

    private Vector3 _resourcePosition;
    private Vector3 _dropOffPointPosition;


    private readonly Subject<Vector3> _targetPositions = new Subject<Vector3>();
    private readonly Subject<Quaternion> _targetRotations = new Subject<Quaternion>();
    private readonly Subject<ResourceType> _gatheredResource = new Subject<ResourceType>();
    private readonly Subject<bool> _isReadyToUnload = new Subject<bool>();

    private GatherResourceOperation _currentGatherkOp;

    private bool _isGathering;

    [Inject]
    private void Init()
    {
        _targetPositions
            .ObserveOnMainThread()
            .Subscribe(StartMovingToPosition);

        _targetRotations
            .ObserveOnMainThread()
            .Subscribe(SetRoation);

        _gatheredResource
            .ObserveOnMainThread()
            .Subscribe(BeginGathering);

        _isReadyToUnload
            .ObserveOnMainThread()
            .Where(x => x == true)
            .Subscribe(x => UnloadResources());

        _animator = GetComponent<Animator>();
        _stopCommandExecutor = GetComponent<StopCommandExecutor>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navigationState = GetComponent<UnitNavigationStateManager>();
        _factionMember = GetComponent<FactionMember>();
    }

    private void StartMovingToPosition(Vector3 position)
    {
        if (!_navMeshAgent.hasPath)
        {
            _navigationState.MakeUnitMovable(true);
            _animator.SetTrigger("Walk");
        }

        _navMeshAgent.destination = position;
        _isGathering = false;
    }

    private void SetRoation(Quaternion targetRotation)
    {
        transform.rotation = targetRotation;
    }

    private void BeginGathering(ResourceType resource)
    {
        lock (this)
        {
            if (_resourceLoad.type != resource)
            {
                _resourceLoad.type = resource;
                _resourceLoad.amount = 0;
                _isGathering = false;
            }
        }

        if (!_isGathering)
        {
            if (_navMeshAgent.isActiveAndEnabled)
            {
                _navMeshAgent.isStopped = true;
                _navMeshAgent.ResetPath();
            }
            _animator.SetTrigger("Gather");
            _isGathering = true;
        }
    }

    private void UnloadResources()
    {
        if (_resourceDropoffPoint != null)
        {
            (ResourceType type, int amount) resource;

            lock (this)
            {
                resource.type = _resourceLoad.type;
                resource.amount = (int)_resourceLoad.amount;
                _resourceLoad.amount = 0;
            }

            _resourceDropoffPoint.DepositResources(resource);
        }
        else
            _currentGatherkOp.Cancel();
    }

    public override async Task ExecuteSpecificCommand(IGatherResourceCommand command)
    {
        _resourcePosition = (command.GatherableResource as Component).transform.position;

        Debug.Log($"<color=#00FF99>{name} goes to gather {command.GatherableResource}</color>");

        try
        {
            _currentGatherkOp = new GatherResourceOperation(this, command.GatherableResource);
            await _currentGatherkOp.WithCancellation(_stopCommandExecutor.CToken);
        }
        catch
        {
            _currentGatherkOp.Cancel();
        }
        _animator.SetTrigger("Idle");
        _navigationState.MakeUnitMovable(false);
        _currentGatherkOp = null;
        _resourceDropoffPoint = null;
    }

    private void Update()
    {
        if (_currentGatherkOp == null)
            return;

        _ourPosition = transform.position;
        _ourRotation = transform.rotation;

        if (_resourceDropoffPoint == null)
            FindDropoffPoint();

        if (_resourceDropoffPoint != null)
        {
            _dropOffPointPosition = (_resourceDropoffPoint as Component).transform.position;
        }
        else
            _dropOffPointPosition = _resourcePosition;

        if (_isGathering)
        lock (this)
        {
            _resourceLoad.amount += Time.deltaTime * _gatherSpeed;
        }
    }

    private void FindDropoffPoint()
    {
        lock (this)
        {
            var dropoffPoints = _resourceRecipientsRepository.FindAll(x => x.FactionID == _factionMember.FactionId);

            Debug.LogWarning(dropoffPoints.Count);

            if (dropoffPoints.Count != 0)
            {
                float distance = float.MaxValue;

                foreach (var dropoffPoint in dropoffPoints)
                {
                    var vector = (dropoffPoint as Component).transform.position - transform.position;
                    if (vector.magnitude < distance)
                    {
                        _resourceDropoffPoint = dropoffPoint;
                        distance = vector.magnitude;
                    }
                }
            }
        }
    }
}
