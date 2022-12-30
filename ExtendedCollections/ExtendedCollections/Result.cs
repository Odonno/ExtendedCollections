namespace ExtendedCollections;

/// <summary>
/// A <see cref="Result{T}"/> structure that contains a boolean for success and the returned value on success.
/// </summary>
/// <typeparam name="T">The type of the returned value.</typeparam>
public class Result<T>
{
    /// <summary>
    /// Whether the result succeeded or not.
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// The returned value when succeeded.
    /// </summary>
    public T Value { get; set; }
}
