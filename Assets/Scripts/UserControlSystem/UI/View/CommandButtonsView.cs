using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CommandButtonsView : MonoBehaviour
{
	public Action<ICommandExecutor> OnClick;

	[SerializeField] private Button _attackButton;
	[SerializeField] private Button _moveButton;
	[SerializeField] private Button _patrolButton;
	[SerializeField] private Button _stopButton;
	[SerializeField] private Button _produceUnitButton;

	private Dictionary<Type, Button> _buttonsByExecutorType;

	private void Start()
	{
		_buttonsByExecutorType = new Dictionary<Type, Button>();
		_buttonsByExecutorType.Add(typeof(CommandExecutorBase<IAttackCommand>), _attackButton);
		_buttonsByExecutorType.Add(typeof(CommandExecutorBase<IMoveCommand>), _moveButton);
		_buttonsByExecutorType.Add(typeof(CommandExecutorBase<IPatrolCommand>), _patrolButton);
		_buttonsByExecutorType.Add(typeof(CommandExecutorBase<IStopCommand>), _stopButton);
		_buttonsByExecutorType.Add(typeof(CommandExecutorBase<IProduceUnitCommand>), _produceUnitButton);
	}

	public void BlockInteractions(ICommandExecutor commandExecutor)
	{
		UnblockAllInteractions();
		GetButtonGameObjectByType(commandExecutor.GetType())
			.GetComponent<Selectable>().interactable = false;
	}

	public void UnblockAllInteractions() => SetInteractible(true);

	private void SetInteractible(bool value)
	{
		_attackButton.GetComponent<Selectable>().interactable = value;
		_moveButton.GetComponent<Selectable>().interactable = value;
		_patrolButton.GetComponent<Selectable>().interactable = value;
		_stopButton.GetComponent<Selectable>().interactable = value;
		_produceUnitButton.GetComponent<Selectable>().interactable = value;
	}

	private GameObject GetButtonGameObjectByType(Type executorInstanceType)
	{
		return _buttonsByExecutorType
			.Where(type => type.Key.IsAssignableFrom(executorInstanceType))
			.First()
			.Value.gameObject;
	}

	public void MakeLayout(IEnumerable<ICommandExecutor> commandExecutors)
	{
		foreach (var currentExecutor in commandExecutors)
		{
			var button = _buttonsByExecutorType
				.Where(type => type
					.Key
					.IsAssignableFrom(currentExecutor.GetType())
					)
				.First()
				.Value;
			button.gameObject.SetActive(true);
			button.onClick.AddListener(() => OnClick?.Invoke(currentExecutor));
		}
	}

	public void Clear()
	{
		foreach (var kvp in _buttonsByExecutorType)
		{
			kvp.Value.GetComponent<Button>().onClick.RemoveAllListeners();
			kvp.Value.gameObject.SetActive(false);
		}
	}
}
