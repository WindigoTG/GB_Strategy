using UnityEngine;
using UnityEngine.UI;
using UniRx;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MenuPresenter : MonoBehaviour
{
	[SerializeField] private Button _backButton;
	[SerializeField] private Button _exitButton;

	private void Start()
	{
		_backButton.OnClickAsObservable()
			.Subscribe(_ =>
			{
				gameObject.SetActive(false);
				Time.timeScale = 1;
			});

#if UNITY_EDITOR
		_exitButton.OnClickAsObservable().Subscribe(_ => EditorApplication.ExitPlaymode());
#else
		_exitButton.OnClickAsObservable().Subscribe(_ => Application.Quit());
#endif
	}
}
