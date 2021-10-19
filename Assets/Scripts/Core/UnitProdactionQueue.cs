using UnityEngine;
using UniRx;
using Zenject;
using System.Collections.Generic;

[RequireComponent(typeof(SetRallyPointCommandExecutor))]
public class UnitProdactionQueue : MonoBehaviour
{
	[SerializeField] private Transform _unitsParent;
	[SerializeField] private int _maximumUnitsInQueue = 5;

	[Inject] DiContainer _diContainer;

	private IRallyPointHolder _rallyPoint;
	private FactionMember _factionMember;

	private ReactiveCollection<IUnitProductionTask> _queue = new ReactiveCollection<IUnitProductionTask>();

	[Inject] ReactiveDictionary<int, Dictionary<ResourceType, int>> _resourcesRepository;

	private void Awake()
	{
		_rallyPoint = GetComponent<SetRallyPointCommandExecutor>();
		_factionMember = GetComponent<FactionMember>();
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

	public void Enqueue(UnitProductionTask task)
    {
		if (_queue.Count < _maximumUnitsInQueue)
		{
			lock (this)
			{
				var factionResources = _resourcesRepository[_factionMember.FactionId];
				bool isEnoughResources = true;

				foreach (var resourceeCost in task.ResourceCost)
					if (factionResources[resourceeCost.Key] < resourceeCost.Value)
					{
						isEnoughResources = false;
						Debug.Log($"<color=#FF9900>Not enough {resourceeCost.Key}</color>");
						break;
					}

				if (isEnoughResources)
				{
					Debug.Log($"<color=#FF00FF>{name} has begun {task.UnitName} construction</color>");

					foreach (var resourceCost in task.ResourceCost)
						factionResources[resourceCost.Key] -= resourceCost.Value;

					_queue.Add(task);
				}
			}
		}
	}

	public void Cancel(int index)
	{
		var innerTask = (UnitProductionTask)_queue[index];
		lock (this)
		{
			var factionResources = _resourcesRepository[_factionMember.FactionId];

			foreach (var resourceCost in innerTask.ResourceCost)
				factionResources[resourceCost.Key] += resourceCost.Value;
		}
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

	public IReadOnlyReactiveCollection<IUnitProductionTask> Queue => _queue;
}
