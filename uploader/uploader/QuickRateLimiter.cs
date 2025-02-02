using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace uploader
{
    internal class QuickRateLimiter
    {
        private class WaiterObj
        {
            public Semaphore Waiter;
            // Constructor
            public WaiterObj(Semaphore waiter)
            { Waiter = waiter; }
        }

        private readonly object _lock = new();
        private readonly int _maxCalls = 1; // one call at a time
        private readonly int _callEverySeconds; // one call per (seconds)
        private readonly List<WaiterObj> _queued = new();
        private readonly List<WaiterObj> _active = new();

        private DateTime _currentIntervalStart;
        private bool _activated = false;

        // Constructor
        public QuickRateLimiter(int callEverySeconds)
        {
            _callEverySeconds = callEverySeconds;
        }

        public void GetQueueLength(out int active, out int activeTotal, out int pending)
        {
            lock (_lock)
            {
                active = _active.Count(item => item.Waiter != null);
                activeTotal = _active.Count;
                pending = _queued.Count;
            }
        }

        private void Enqueue(WaiterObj wo)
        {
            if (_active.Count > _maxCalls)
            {
                throw new InvalidOperationException($"Active count: ({_active.Count}) is out of bounds: ({_maxCalls})");
            }
            else if (_active.Count == _maxCalls)
            {
                _queued.Add(wo);
            }
            else // _active.Count == 0
            {
                // Do call
                DoCall(wo);
            }
        }

        public Semaphore GetWaiter()
        {
            lock (_lock)
            {
                if (!_activated)
                {
                    _activated = true;
                    ResetInterval();
                }

                WaiterObj wo = new(new Semaphore(0, 1));
                Enqueue(wo);
                return wo.Waiter;
            }
        }

        private void ResetInterval()
        {
            _currentIntervalStart = DateTime.UtcNow;
            _active.RemoveAll(item => item.Waiter == null);
        }

        public void TimeTick()
        {
            lock (_lock)
            {
                if (!_activated) return;

                var now = DateTime.UtcNow;
                if ((now - _currentIntervalStart).TotalSeconds >= _callEverySeconds)
                {
                    ResetInterval();

                    var temp = _queued.ToList();
                    foreach (var wo in temp)
                    {
                        if (_active.Count < _maxCalls)
                        {
                            _queued.Remove(wo);
                            // Do call
                            DoCall(wo);
                        }
                        else
                        {
                            break;
                        }
                    }
                    temp.Clear();
                }
            }
        }

        private void DoCall(WaiterObj wo)
        {
            _active.Add(wo);
            wo.Waiter.Release(1);
        }

        public void Clear()
        {
            lock (_lock)
            {
                foreach (var wo in _active)
                {
                    wo.Waiter?.Dispose();
                }
                _active.Clear();

                foreach (var wo in _queued)
                {
                    wo.Waiter.Dispose();
                }
                _queued.Clear();
            }
        }

        public void ReleaseWaiter(Semaphore waiter)
        {
            lock (_lock)
            {
                if (waiter == null) throw new InvalidOperationException($"Waiter is null");
                waiter.Dispose();

                int index = _active.FindIndex(item => item.Waiter == waiter);
                if (index != -1)
                {
                    _active[index].Waiter = null;
                }
                else if (_queued.Any(item => item.Waiter == waiter))
                {
                    _queued.RemoveAll(item => item.Waiter == waiter);
                }
            }
        }
    }
}
