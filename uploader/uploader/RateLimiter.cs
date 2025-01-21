using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace uploader
{
    internal class RateLimiter
    {
        private class WaiterObj
        {
            public Semaphore Waiter;
            public bool Upload;
            // Constructor
            public WaiterObj(Semaphore waiter, bool upload)
            { Waiter = waiter; Upload = upload; }
        }

        private readonly object _lock = new();
        private readonly int _callsPerMinute;
        private readonly List<WaiterObj> _queued = new();
        private readonly List<WaiterObj> _active = new();

        private DateTime _currentMinuteStart;
        private bool _activated = false;

        // Constructor
        public RateLimiter(int callsPerMinute)
        {
            _callsPerMinute = callsPerMinute;
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

        private int ActiveUploads()
        {
            return _active.Count(item => item.Upload && item.Waiter != null);
        }

        private int QueuedNonUploads()
        {
            return _queued.Count(item => !item.Upload);
        }

        private void Enqueue(WaiterObj wo)
        {
            if (_active.Count > _callsPerMinute)
            {
                throw new InvalidOperationException($"Active count: ({_active.Count}) is out of bounds: ({_callsPerMinute})");
            }
            else if (_active.Count == _callsPerMinute)
            {
                _queued.Add(wo);
            }
            else // _active.Count < _callsPerMinute
            {
                if (!wo.Upload || ActiveUploads() == 0)
                {
                    // Do call
                    DoCall(wo);
                }
                else
                {
                    _queued.Add(wo);
                }
            }
        }

        public Semaphore GetWaiter(bool upload)
        {
            lock (_lock)
            {
                if (!_activated)
                {
                    _activated = true;
                    ResetMinute();
                }

                WaiterObj wo = new(new Semaphore(0, 1), upload);
                Enqueue(wo);
                return wo.Waiter;
            }
        }

        private void ResetMinute()
        {
            _currentMinuteStart = DateTime.UtcNow;
            _active.RemoveAll(item => item.Waiter == null);
        }

        public void TimeTick()
        {
            lock (_lock)
            {
                if (!_activated) return;

                var now = DateTime.UtcNow;
                if ((now - _currentMinuteStart).TotalMinutes >= 1.05) // minute padding just in case
                {
                    ResetMinute();

                    var temp = _queued.ToList();
                    foreach (var wo in temp)
                    {
                        if (_active.Count < _callsPerMinute)
                        {
                            if (!wo.Upload || ActiveUploads() == 0)
                            {
                                _queued.Remove(wo);
                                // Do call
                                DoCall(wo);
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    temp.Clear();
                }
                else
                {
                    // activate uploads quickly if only uploads are queued
                    // otherwise will end up waiting a minute after each upload despite having free rate slots
                    if (_active.Count < _callsPerMinute && _queued.Count > 0 &&
                         ActiveUploads() == 0 && QueuedNonUploads() == 0)
                    {
                        var wo = _queued[0];
                        _queued.RemoveAt(0);
                        // Do call
                        DoCall(wo);
                    }
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
    } // class RateLimiter
}
