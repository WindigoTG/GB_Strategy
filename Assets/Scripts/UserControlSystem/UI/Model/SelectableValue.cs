using System;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(SelectableValue), menuName = "Strategy Game/" + nameof(SelectableValue), order = 0)]
public class SelectableValue : ScriptableObject
{
	public ISelectable CurrentValue { get; private set; }
	public Action<ISelectable> OnSelected;

	private Outline _outline;

	public void SetValue(ISelectable value, Outline outline)
	{
		CurrentValue = value;
		OnSelected?.Invoke(value);

		_outline = outline;

		if (_outline != null)
			_outline.enabled = true;
	}

	public void Deselect()
    {
		if (_outline != null)
			_outline.enabled = false;
	}
}
