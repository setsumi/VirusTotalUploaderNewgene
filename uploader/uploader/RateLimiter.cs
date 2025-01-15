using System;
using System.Collections.Generic;
using System.Threading;

namespace uploader
{
    internal class RateLimiter
    {
        private readonly object _lock = new object();
        private readonly Queue<Semaphore> _queue = new Queue<Semaphore>();
        private readonly int _callsPerMinute;

        private DateTime _currentMinuteStart;
        private int _callsInCurrentMinute;
        private bool _active = false;

        // Constructor
        public RateLimiter(int callsPerMinute)
        {
            _callsPerMinute = callsPerMinute;
        }

        public int GetQueueLength()
        {
            lock (_lock)
            {
                return _queue.Count;
            }
        }

        private void Enqueue(Semaphore waiter)
        {
            if (_callsInCurrentMinute < _callsPerMinute)
            {
                // Do call
                _callsInCurrentMinute++;
                waiter.Release(1);
            }
            else if (_callsInCurrentMinute == _callsPerMinute)
            {
                _queue.Enqueue(waiter);
            }
            else
            {
                throw new InvalidOperationException($"Calls in current minute: ({_callsInCurrentMinute}) is out of bounds: ({_callsPerMinute})");
            }
        }

        public Semaphore GetWaiter()
        {
            lock (_lock)
            {
                if (!_active)
                {
                    _active = true;
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
            _callsInCurrentMinute = 0;
        }

        public void TimeTick()
        {
            lock (_lock)
            {
                if (!_active) return;

                var now = DateTime.UtcNow;
                if ((now - _currentMinuteStart).TotalMinutes >= 1.05) // minute padding just in case
                {
                    ResetMinute();

                    for (int i = 0; i < _callsPerMinute; i++)
                    {
                        Semaphore waiter = null;
                        try { waiter = _queue.Dequeue(); }
                        catch (InvalidOperationException) { break; }

                        // Do call
                        _callsInCurrentMinute++;
                        waiter.Release(1);
                    }
                }
            }
        }

        public void Clear()
        {
            lock (_lock)
            {
                foreach (var waiter in _queue)
                {
                    waiter.Dispose();
                }
                _queue.Clear();
            }
        }
    } // class RateLimiter
}
