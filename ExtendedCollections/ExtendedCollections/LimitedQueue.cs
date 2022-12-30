using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace ExtendedCollections
{
    /// <summary>
    /// A standard <see cref="Queue{T}"/> with a maximum number of items inside.
    /// </summary>
    /// <typeparam name="T">The type of item stored.</typeparam>
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
        /// Event triggered when an element is enqueued.
        /// </summary>
        public event EventHandler Enqueued;

        /// <summary>
        /// Event triggered when an element is dequeued.
        /// </summary>
        public event EventHandler Dequeued;

        /// <summary>
        /// Creates a new instance of a <see cref="LimitedQueue{T}"/>.
        /// </summary>
        /// <param name="limit">The maximum item inside the queue at any given time.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when limit is less than or equal to 0.</exception>
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
        public Result<T> TryDequeue()
        {
            bool success = _queue.TryDequeue(out var item);
            if (success)
            {
                Dequeued?.Invoke(this, EventArgs.Empty);
            }

            return new Result<T> { Success = success, Value = item };
        }

        /// <summary>
        /// Creates a <see cref="List{T}"/> from the inner <see cref="Queue{T}"/>.
        /// </summary>
        /// <returns>A <see cref="List{T}"/> from the inner <see cref="Queue{T}"/>.</returns>
        public List<T> ToList()
        {
            return _queue.ToList();
        }
    }
}
