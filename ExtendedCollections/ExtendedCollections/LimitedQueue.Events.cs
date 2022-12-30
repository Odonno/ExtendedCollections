using System;

namespace ExtendedCollections
{
    public class EnqueuedEventArgs<T> : EventArgs
    {
        public T Item { get; set; }
    }

    public class DequeuedEventArgs<T> : EventArgs
    {
        public T Item { get; set; }
    }
}
