using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using UniRx;
using System;

public class BottomLeftPresenter : MonoBehaviour
{
	[SerializeField] private Image _selectedImage;
	[SerializeField] private Slider _healthSlider;
	[SerializeField] private TextMeshProUGUI _text;
	[SerializeField] private Image _sliderBackground;
	[SerializeField] private Image _sliderFillImage;

	[Inject] private IObservable<ISelectable> _selectedValue;

	private ISelectable _currentSelectable;
	private IDisposable _observation;

	private void Start()
	{
		_selectedValue.Subscribe(OnSelected);
	}

	private void OnSelected(ISelectable selected)
	{
		_observation?.Dispose();
		_currentSelectable = null;
		_selectedImage.enabled = selected != null;
		_healthSlider.gameObject.SetActive(selected != null);
		_text.enabled = selected != null;

		if (selected != null)
		{
			_currentSelectable = selected;

			_selectedImage.sprite = selected.Icon;
			_healthSlider.minValue = 0;
			_healthSlider.maxValue = selected.MaxHealth;

			_observation = selected.ObservableHealth.Subscribe(UpdateHealth);

			UpdateHealth(selected.CurrentHealth);
		}
	}

	private void UpdateHealth(float value)
    {
		_text.text = $"{value}/{_currentSelectable.MaxHealth}";
		_healthSlider.value = value;
		var color = Color.Lerp(Color.red, Color.green, value / (float)_currentSelectable.MaxHealth);
		_sliderBackground.color = color * 0.5f;
		_sliderFillImage.color = color;

		if (value <= 0)
			OnSelected(null);
	}
}
