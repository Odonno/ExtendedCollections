namespace ExtendedCollections;

public class EntityAddedEventArgs<T> : EventArgs
{
    public T Entity { get; set; }
}

public class EntityUpdatedEventArgs<T> : EventArgs
{
    public T Entity { get; set; }
}

public class EntityRemovedEventArgs<T> : EventArgs
{
    public T Entity { get; set; }
}
