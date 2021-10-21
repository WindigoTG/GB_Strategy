using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public partial class GatherResourceCommandExecutor : CommandExecutorBase<IGatherResourceCommand>
{
    public class GatherResourceOperation : IAwaitable<AsyncExtensions.Void>
    {
        public class GatherOperationAwaiter : AwaiterBase<AsyncExtensions.Void>
        {
            private GatherResourceOperation _gatherOperation;

            public GatherOperationAwaiter(GatherResourceOperation gatherOperation)
            {
                _gatherOperation = gatherOperation;
                _gatherOperation.OnComplete += OnComplete;
            }

            private void OnComplete()
            {
                _gatherOperation.OnComplete -= OnComplete;
                OnAwaitedEvent(new AsyncExtensions.Void());
            }
        }

        private event Action OnComplete;
        private bool _isCancelled;

        private IGatherable _resource;
        private GatherResourceCommandExecutor _gatherResourceCommandExecutor;

        public GatherResourceOperation(GatherResourceCommandExecutor gatherResourceCommandExecutor, IGatherable resource)
        {
            _resource = resource;
            _gatherResourceCommandExecutor = gatherResourceCommandExecutor;

            var thread = new Thread(GatherAlgorythm);
            thread.Start();
        }

        private void GatherAlgorythm()
        {
            while (true)
            {
                if (_gatherResourceCommandExecutor == null ||
                    _isCancelled)
                {
                    OnComplete?.Invoke();
                    return;
                }

                var resourcePosition = default(Vector3);
                var dropoffPointPosition = default(Vector3);
                var ourPosition = default(Vector3);
                var ourRotation = default(Quaternion);
                var currentloadAmount = default(float);

                lock (_gatherResourceCommandExecutor)
                {
                    resourcePosition = _gatherResourceCommandExecutor._resourcePosition;
                    dropoffPointPosition = _gatherResourceCommandExecutor._dropOffPointPosition;
                    ourPosition = _gatherResourceCommandExecutor._ourPosition;
                    ourRotation = _gatherResourceCommandExecutor._ourRotation;
                    currentloadAmount = _gatherResourceCommandExecutor._resourceLoad.amount;
                }

                var isFull = currentloadAmount >= _gatherResourceCommandExecutor._maximumLoad;

                var destination = default(Vector3);

                if (isFull)
                {
                    destination = dropoffPointPosition;
                }
                else
                    destination = resourcePosition;

                var vector = destination - ourPosition;
                var distanceToTarget = vector.magnitude;

                if (distanceToTarget > _gatherResourceCommandExecutor._interactionDistance)
                {
                    _gatherResourceCommandExecutor._targetPositions.OnNext(destination);
                    Thread.Sleep(100);
                }
                else if (ourRotation != Quaternion.LookRotation(vector))
                {
                    _gatherResourceCommandExecutor._targetRotations
                        .OnNext(Quaternion.LookRotation(vector));
                }
                else
                {
                    if (isFull)
                    {
                        _gatherResourceCommandExecutor._isReadyToUnload.OnNext(true);
                    }
                    else
                    {
                        _gatherResourceCommandExecutor._gatheredResource.OnNext(_resource.ResourceType);
                    }
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
            return new GatherOperationAwaiter(this);
        }
    }
}
