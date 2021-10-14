using UnityEngine;
using UniRx;
using Random = UnityEngine.Random;
using System.Threading.Tasks;
using Zenject;

[RequireComponent(typeof(SetRallyPointCommandExecutor))]
public class ProduceUnitCommandExecutor : CommandExecutorBase<IProduceUnitCommand>, IUnitProducer
{
	[SerializeField] private Transform _unitsParent;
	[SerializeField] private int _maximumUnitsInQueue = 5;

	[Inject] DiContainer _diContainer;

	private IRallyPointHolder _rallyPoint;

	private ReactiveCollection<IUnitProductionTask> _queue = new ReactiveCollection<IUnitProductionTask>();

	public IReadOnlyReactiveCollection<IUnitProductionTask> Queue => _queue;

	private void Awake()
	{
		_rallyPoint = GetComponent<SetRallyPointCommandExecutor>();
	}

	private void Update()
	{
		if (_queue.Count == 0)
		{
			return;
		}

		var innerTask = (UnitProductionTask)_queue[0];
		innerTask.TimeLeft -= Time.deltaTime;
		innerTask.TimeLeft -= Time.deltaTime;
		if (innerTask.TimeLeft <= 0)
		{
			RemoveTaskAtIndex(0);
			var newUnit = _diContainer.InstantiatePrefab(innerTask.UnitPrefab,
				new Vector3(
					transform.position.x + Random.Range(-3f, 3),
					0,
					transform.position.z + Random.Range(-3f, 3f)),
				Quaternion.identity, _unitsParent);
			Debug.Log($"<color=#00FF00>Construction complete</color>");

			var factionMember = newUnit.GetComponent<FactionMember>();
			factionMember.SetFaction(GetComponent<FactionMember>().FactionId);

			if (_rallyPoint.IsRallyPointSet)
			{
				newUnit.GetComponent<ICommandsQueue>().EnqueueCommand(new MoveCommand(_rallyPoint.RallyPoint));
			}
		}
	}

	public void Cancel(int index)
	{
		var innerTask = (UnitProductionTask)_queue[index];
		Debug.Log($"<color=#FF9900>Construction of {innerTask.UnitName} has been cancelled</color>");
		RemoveTaskAtIndex(index); 
	}

	private void RemoveTaskAtIndex(int index)
	{
		for (int i = index; i < _queue.Count - 1; i++)
    		{
			_queue[i] = _queue[i + 1];
		}
		_queue.RemoveAt(_queue.Count - 1);
	}

	public override async Task ExecuteSpecificCommand(IProduceUnitCommand command)
	{
		if (_queue.Count < _maximumUnitsInQueue)
		{
			Debug.Log($"<color=#FF00FF>{name} has begun {command.UnitPrefab.name} construction</color>");
			_queue.Add(new UnitProductionTask(command.ProductionTime, command.Icon, command.UnitPrefab, command.UnitName));
			await Task.CompletedTask;
		}
	}
}
