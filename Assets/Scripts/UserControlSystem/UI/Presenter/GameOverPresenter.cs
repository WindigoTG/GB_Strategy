using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UniRx;
using Zenject;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameOverPresenter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _winnerText;
    [SerializeField] private Button _quitButton;
    [SerializeField] private GameObject _messageWindow;

    [Inject]
    private readonly Dictionary<int, int> _factionMemberCounter;
    private Subject<int> _factionsCount = new Subject<int>();
    private bool _isGameOver;

    void Start()
    {
        _messageWindow.SetActive(false);

        _factionsCount.ObserveOnMainThread().Subscribe(ProcessCountResult);

#if UNITY_EDITOR
        _quitButton.OnClickAsObservable().Subscribe(_ => EditorApplication.ExitPlaymode());
#else
		_quitButton.OnClickAsObservable().Subscribe(_ => Application.Quit());
#endif

        ThreadPool.QueueUserWorkItem(MonitorFactionsCount);
    }

    private void ProcessCountResult(int count)
    {
        if (count >= 2)
            return;

        _isGameOver = true;

        if (count == 0)
            _winnerText.text = "Draw.";
        else
        {
            int factionID;
            lock (this)
            {
                factionID = _factionMemberCounter.Keys.First();
            }
            _winnerText.text = $"Faction {factionID} has won!";
        }

        _messageWindow.SetActive(true);
    }

    private void MonitorFactionsCount(object state)
    {
        while (!_isGameOver)
        {
            Thread.Sleep(1000);

            lock (this)
            {
                _factionsCount.OnNext(_factionMemberCounter.Keys.Count);
            }
        }
    }
}
