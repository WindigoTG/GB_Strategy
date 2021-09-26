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
	[SerializeField] private Button _holdPositionButton;
	[SerializeField] private Button _stopButton;
	[SerializeField] private Button _produceUnitButton;

	private Dictionary<Type, Button> _buttonsByExecutorType;

	private void Start()
	{
		_buttonsByExecutorType = new Dictionary<Type, Button>();
		_buttonsByExecutorType.Add(typeof(CommandExecutorBase<IAttackCommand>), _attackButton);
		_buttonsByExecutorType.Add(typeof(CommandExecutorBase<IMoveCommand>), _moveButton);
		_buttonsByExecutorType.Add(typeof(CommandExecutorBase<IPatrolCommand>), _patrolButton);
		_buttonsByExecutorType.Add(typeof(CommandExecutorBase<IHoldPositionCommand>), _holdPositionButton);
		_buttonsByExecutorType.Add(typeof(CommandExecutorBase<IStopCommand>), _stopButton);
		_buttonsByExecutorType.Add(typeof(CommandExecutorBase<IProduceUnitCommand>), _produceUnitButton);
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
