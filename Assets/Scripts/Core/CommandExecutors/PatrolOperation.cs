using System;
using System.Threading;
using UnityEngine;

public partial class PatrolCommandExecutor : CommandExecutorBase<IPatrolCommand>
{
    public class PatrolOperation : IAwaitable<AsyncExtensions.Void>
    {
        public class PatrolOperationAwaiter : AwaiterBase<AsyncExtensions.Void>
        {
            private PatrolOperation _attackOperation;

            public PatrolOperationAwaiter(PatrolOperation attackOperation)
            {
                _attackOperation = attackOperation;
                attackOperation.OnComplete += OnComplete;
            }

            private void OnComplete()
            {
                _attackOperation.OnComplete -= OnComplete;
                OnAwaitedEvent(new AsyncExtensions.Void());
            }
        }

        private event Action OnComplete;

        private readonly PatrolCommandExecutor _patrolCommandExecutor;

        private bool _isCancelled;

        public PatrolOperation(PatrolCommandExecutor patrolCommandExecutor)
        {
            _patrolCommandExecutor = patrolCommandExecutor;

            SetNextDestination();

            var thread = new Thread(PatrolAlgorythm);
            thread.Start();
        }

        private void SetNextDestination()
        {
            var destination = _patrolCommandExecutor._patrolPoints.Dequeue();

            _patrolCommandExecutor._targetPositions.OnNext(destination);

            _patrolCommandExecutor._patrolPoints.Enqueue(destination);
        }

        private void PatrolAlgorythm(object obj)
        {
            while (true)
            {
                if (_isCancelled)
                {
                    OnComplete?.Invoke();
                    return;
                }

                if (_patrolCommandExecutor._hasReachedDestination)
                {
                    SetNextDestination();
                    Thread.Sleep(100);
                }
            }
        }

        public void Cancel()
        {
            _isCancelled = true;
            OnComplete?.Invoke();
        }

        public IAwaiter<AsyncExtensions.Void> GetAwaiter()
        {
            return new PatrolOperationAwaiter(this);
        }
    }
}
