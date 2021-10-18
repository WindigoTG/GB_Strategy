using UnityEngine;
using UniRx;

public class AutoAttackAgent : MonoBehaviour
{
    [SerializeField] private UnitCommandsQueue _queue;

    private void Start()
    {
        AutoAttackEvaluator.AutoAttackCommands
            .ObserveOnMainThread()
            .Where(command => command.Attacker == gameObject)
            .Where(command => command.Attacker != null && command.Target != null)
            .Subscribe(command => AutoAttack(command.Target))
            .AddTo(this);
    }

    private void AutoAttack(GameObject target)
    {
        _queue.Clear();
        _queue.EnqueueCommand(new AutoAttackCommand(target.GetComponent<IAttackable>()));
    }
}
