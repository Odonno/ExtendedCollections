using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace ExtendedCollections
{
    public class LimitedStack<T>
    {
        private readonly ConcurrentStack<T> _stack = new ConcurrentStack<T>();

        /// <summary>
        /// Limit of the stack, meaning max number of items in the stack.
        /// </summary>
        public int Limit { get; }

        /// <summary>
        /// Number of items in the stack.
        /// </summary>
        public int Count => _stack.Count;

        /// <summary>
        /// Whether there is no item in the stack.
        /// </summary>
        public bool IsEmpty => _stack.IsEmpty;

        /// <summary>
        /// Values inside the stack.
        /// </summary>
        public List<T> Values => _stack.ToList();

        /// <summary>
        /// Event triggered when an element is pushed.
        /// </summary>
        public event EventHandler Pushed;

        /// <summary>
        /// Event triggered when an element is popped.
        /// </summary>
        public event EventHandler Popped;

        public LimitedStack(int limit)
        {
            if (limit <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(limit), "The limit should be a positive integer.");
            }

            Limit = limit;

            Pushed += HandlePushed;
        }

        private void HandlePushed(object sender, EventArgs e)
        {
            while (Count > Limit)
            {
                TryPop();
            }
        }

        /// <summary>
        /// Push an item in the stack.
        /// </summary>
        /// <param name="item">Item to push.</param>
        public void Push(T item)
        {
            _stack.Push(item);
            Pushed?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Pop an item (if possible).
        /// </summary>
        /// <returns></returns>
        public Tuple<bool, T> TryPop()
        {
            bool success = _stack.TryPop(out var item);

            if (success)
            {
                Popped?.Invoke(this, EventArgs.Empty);
            }

            return Tuple.Create(success, item);
        }
    }
}
