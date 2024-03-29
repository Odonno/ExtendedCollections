﻿namespace ExtendedCollections;

/// <summary>
/// A standard <see cref="Stack{T}"/> with a maximum number of items inside.
/// </summary>
/// <typeparam name="T">The type of item stored.</typeparam>
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
    /// Event triggered when an element is pushed.
    /// </summary>
    public event EventHandler<PushedEventArgs<T>> Pushed;

    /// <summary>
    /// Event triggered when an element is popped.
    /// </summary>
    public event EventHandler<PoppedEventArgs<T>> Popped;

    /// <summary>
    /// Creates a new instance of a <see cref="LimitedStack{T}"/>.
    /// </summary>
    /// <param name="limit">The maximum item inside the stack at any given time.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when limit is less than or equal to 0.</exception>
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
        Pushed?.Invoke(this, new PushedEventArgs<T> { Item = item });
    }

    /// <summary>
    /// Pop an item (if possible).
    /// </summary>
    /// <returns></returns>
    public Result<T> TryPop()
    {
        bool success = _stack.TryPop(out var item);
        if (success)
        {
            Popped?.Invoke(this, new PoppedEventArgs<T> { Item = item });
        }

        return new Result<T> { Success = success, Value = item };
    }

    /// <summary>
    /// Creates a <see cref="List{T}"/> from the inner <see cref="Stack{T}"/>.
    /// </summary>
    /// <returns>A <see cref="List{T}"/> from the inner <see cref="Stack{T}"/>.</returns>
    public List<T> ToList()
    {
        return _stack.ToList();
    }
}
