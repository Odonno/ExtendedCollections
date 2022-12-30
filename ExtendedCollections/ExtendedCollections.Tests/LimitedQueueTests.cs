namespace ExtendedCollections.Tests;

public class LimitedQueueTests
{
    [Fact]
    public void CannotExceed5Items()
    {
        // Arrange
        int queueLimit = 5;
        var queue = new LimitedQueue<int>(queueLimit);

        int enqueuedEvents = 0;
        int dequeuedEvents = 0;

        queue.Enqueued += (sender, e) =>
        {
            enqueuedEvents++;
            Assert.True(queue.Count <= 5);
        };
        queue.Dequeued += (sender, e) =>
        {
            dequeuedEvents++;
            Assert.True(queue.Count <= 5);
        };

        // Act
        queue.Enqueue(1);
        queue.Enqueue(2);
        queue.Enqueue(3);
        queue.Enqueue(4);
        queue.Enqueue(5);
        queue.Enqueue(6);

        // Assert
        Assert.Equal(6, enqueuedEvents);
        Assert.Equal(1, dequeuedEvents);

        var values = queue.ToList();

        Assert.Equal(2, values[0]);
        Assert.Equal(6, values[4]);
    }

    [Fact]
    public void ThrowExceptionIfQueueLimitIs0()
    {
        // Arrange
        int queueLimit = 0;

        // Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => new LimitedQueue<int>(queueLimit));
    }

    [Fact]
    public void ThrowExceptionIfQueueLimitIsNegative()
    {
        // Arrange
        int queueLimit = -10;

        // Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => new LimitedQueue<int>(queueLimit));
    }

    [Fact]
    public void CanDequeueSuccessfully()
    {
        // Arrange
        int queueLimit = 5;
        var queue = new LimitedQueue<int>(queueLimit);

        int enqueuedEvents = 0;
        int dequeuedEvents = 0;

        queue.Enqueued += (sender, e) =>
        {
            enqueuedEvents++;
        };
        queue.Dequeued += (sender, e) =>
        {
            dequeuedEvents++;
        };

        // Act
        queue.Enqueue(1);
        var result = queue.TryDequeue();

        // Assert
        Assert.Equal(1, enqueuedEvents);
        Assert.Equal(1, dequeuedEvents);

        Assert.True(result.Success);
        Assert.Equal(1, result.Value);
    }

    [Fact]
    public void CannotDequeue()
    {
        // Arrange
        int queueLimit = 5;
        var queue = new LimitedQueue<int>(queueLimit);

        int enqueuedEvents = 0;
        int dequeuedEvents = 0;

        queue.Enqueued += (sender, e) =>
        {
            enqueuedEvents++;
        };
        queue.Dequeued += (sender, e) =>
        {
            dequeuedEvents++;
        };

        // Act
        var result = queue.TryDequeue();

        // Assert
        Assert.Equal(0, enqueuedEvents);
        Assert.Equal(0, dequeuedEvents);

        Assert.False(result.Success);
    }
}
