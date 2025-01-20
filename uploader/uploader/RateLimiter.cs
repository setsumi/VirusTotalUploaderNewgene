using System;
using System.Collections.Generic;
using System.Threading;

namespace uploader
{
    internal class RateLimiter
    {
        private readonly object _lock = new object();
        private readonly int _callsPerMinute;
        private Queue<Semaphore> _queued = new Queue<Semaphore>();
        private readonly List<Semaphore> _active = new List<Semaphore>();

        private DateTime _currentMinuteStart;
        private bool _activated = false;

        // Constructor
        public RateLimiter(int callsPerMinute)
        {
            _callsPerMinute = callsPerMinute;
        }

        public void GetQueueLength(out int active, out int pending)
        {
            lock (_lock)
            {
                active = _active.Count;
                pending = _queued.Count;
            }
        }

        private void Enqueue(Semaphore waiter)
        {
            if (_active.Count < _callsPerMinute)
            {
                // Do call
                _active.Add(waiter);
                waiter.Release(1);
            }
            else if (_active.Count == _callsPerMinute)
            {
                _queued.Enqueue(waiter);
            }
            else
            {
                throw new InvalidOperationException($"Active count: ({_active.Count}) is out of bounds: ({_callsPerMinute})");
            }
        }

        public Semaphore GetWaiter()
        {
            lock (_lock)
            {
                if (!_activated)
                {
                    _activated = true;
                    ResetMinute();
                }

                Semaphore waiter = new Semaphore(0, 1);
                Enqueue(waiter);
                return waiter;
            }
        }

        private void ResetMinute()
        {
            _currentMinuteStart = DateTime.UtcNow;
            _active.RemoveAll(item => item == null);
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

                    for (int i = _active.Count; i < _callsPerMinute; i++)
                    {
                        Semaphore waiter = null;
                        try { waiter = _queued.Dequeue(); }
                        catch (InvalidOperationException) { break; }

                        // Do call
                        _active.Add(waiter);
                        waiter.Release(1);
                    }
                }
            }
        }

        public void Clear()
        {
            lock (_lock)
            {
                foreach (var waiter in _active)
                {
                    waiter?.Dispose();
                }
                _active.Clear();

                foreach (var waiter in _queued)
                {
                    waiter.Dispose();
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

                if (_active.Contains(waiter))
                {
                    _active[_active.IndexOf(waiter)] = null;
                }
                else if (_queued.Contains(waiter))
                {
                    var q = new Queue<Semaphore>();
                    foreach (var item in _queued)
                    {
                        if (item != waiter)
                            q.Enqueue(item);
                    }
                    _queued.Clear();
                    _queued = q;
                }
            }
        }
    } // class RateLimiter
}
