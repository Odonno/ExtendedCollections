using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace ExtendedCollections
{
    public class LimitedQueue<T>
    {
        private readonly ConcurrentQueue<T> _queue = new ConcurrentQueue<T>();

        /// <summary>
        /// Limit of the queue, meaning max number of items in the queue.
        /// </summary>
        public int Limit { get; }

        /// <summary>
        /// Number of items in the queue.
        /// </summary>
        public int Count => _queue.Count;

        /// <summary>
        /// Whether there is no item in the queue.
        /// </summary>
        public bool IsEmpty => _queue.IsEmpty;

        /// <summary>
        /// Values inside the queue.
        /// </summary>
        public List<T> Values => _queue.ToList();

        /// <summary>
        /// Event triggered when an element is enqueued.
        /// </summary>
        public event EventHandler Enqueued;

        /// <summary>
        /// Event triggered when an element is dequeued.
        /// </summary>
        public event EventHandler Dequeued;

        public LimitedQueue(int limit)
        {
            if (limit <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(limit), "The limit should be a positive integer.");
            }

            Limit = limit;

            Enqueued += HandleEnqueued;
        }

        private void HandleEnqueued(object sender, EventArgs e)
        {
            while (Count > Limit)
            {
                TryDequeue();
            }
        }

        /// <summary>
        /// Enqueue a new item.
        /// </summary>
        /// <param name="item">Item to enqueue.</param>
        public void Enqueue(T item)
        {
            _queue.Enqueue(item);
            Enqueued?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Dequeue an item (if possible).
        /// </summary>
        /// <returns>Returns whether the dequeue has been done, with the element dequeued.</returns>
        public Tuple<bool, T> TryDequeue()
        {
            bool success = _queue.TryDequeue(out var item);

            if (success)
            {
                Dequeued?.Invoke(this, EventArgs.Empty);
            }

            return Tuple.Create(success, item);
        }
    }
}
