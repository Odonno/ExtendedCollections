namespace ExtendedCollections;

public class PushedEventArgs<T> : EventArgs
{
    public T Item { get; set; }
}

public class PoppedEventArgs<T> : EventArgs
{
    public T Item { get; set; }
}
