namespace ExtendedCollections.Tests;

public class EntityListTests
{
    public class Entity
    {
        public int Id { get; set; }
        public string Property { get; set; }
    }

    [Fact]
    public void UpsertAnExistingItem()
    {
        // Arrange
        var entityList = new EntityList<int, Entity>(e => e.Id);

        int id = 1;

        int addedEvents = 0;
        int updatedEvents = 0;

        entityList.Added += (sender, e) =>
        {
            addedEvents++;
        };
        entityList.Updated += (sender, e) =>
        {
            updatedEvents++;
        };

        // Act
        var entity1 = new Entity
        {
            Id = id,
            Property = "Created"
        };
        entityList.Upsert(entity1);
        var newEntity1 = new Entity
        {
            Id = id,
            Property = "Updated"
        };
        entityList.Upsert(newEntity1);

        // Assert
        Assert.Equal(1, addedEvents);
        Assert.Equal(1, updatedEvents);

        Assert.Single(entityList);
        Assert.Equal(1, entityList[id].Id);
        Assert.Equal("Updated", entityList[id].Property);
    }

    [Fact]
    public void FindAnExistingItem()
    {
        // Arrange
        var entityList = new EntityList<int, Entity>(e => e.Id);

        // Act
        var entity1 = new Entity
        {
            Id = 1,
            Property = "Created"
        };
        entityList.Upsert(entity1);

        var entityFound = entityList.Find(1);

        // Assert
        Assert.Single(entityList);
        Assert.Equal(1, entityFound.Id);
        Assert.Equal("Created", entityFound.Property);
    }

    [Fact]
    public void RemoveAnExistingItem()
    {
        // Arrange
        var entityList = new EntityList<int, Entity>(e => e.Id);

        var entity1 = new Entity
        {
            Id = 1,
            Property = "Created"
        };
        entityList.Upsert(entity1);

        int removedEvents = 0;

        entityList.Removed += (sender, e) =>
        {
            removedEvents++;
        };

        // Act
        entityList.Remove(1);

        // Assert
        Assert.Equal(1, removedEvents);
        Assert.Empty(entityList);
    }

    [Fact]
    public void RemoveNothing()
    {
        // Arrange
        var entityList = new EntityList<int, Entity>(e => e.Id);

        var entity1 = new Entity
        {
            Id = 1,
            Property = "Created"
        };
        entityList.Upsert(entity1);

        int removedEvents = 0;

        entityList.Removed += (sender, e) =>
        {
            removedEvents++;
        };

        // Act
        entityList.Remove(4);

        // Assert
        Assert.Equal(0, removedEvents);
        Assert.NotEmpty(entityList);
    }

    [Fact]
    public void Count_Should_Be_2_When_Upserting_Two_Items()
    {
        // Arrange
        var entityList = new EntityList<int, Entity>(e => e.Id);

        // Act
        var entity1 = new Entity
        {
            Id = 1,
            Property = "One"
        };
        entityList.Upsert(entity1);

        var entity2 = new Entity
        {
            Id = 2,
            Property = "Two"
        };
        entityList.Upsert(entity2);

        // Assert
        Assert.Equal(2, entityList.Count);
    }

    [Fact]
    public void Count_Should_Be_1_When_Upserting_Same_Entity()
    {
        // Arrange
        var entityList = new EntityList<int, Entity>(e => e.Id);

        // Act
        var entity1 = new Entity
        {
            Id = 1,
            Property = "One"
        };
        entityList.Upsert(entity1);
        entityList.Upsert(entity1);

        // Assert
        Assert.Equal(1, entityList.Count);
    }

    [Fact]
    public void Keys()
    {
        // Arrange
        var entityList = new EntityList<int, Entity>(e => e.Id);

        // Act
        var entity1 = new Entity
        {
            Id = 1,
            Property = "One"
        };
        entityList.Upsert(entity1);

        var entity2 = new Entity
        {
            Id = 2,
            Property = "Two"
        };
        entityList.Upsert(entity2);

        // Assert
        var expected = new List<int> { 1, 2 };
        Assert.Equal(expected, entityList.Keys);
    }

    [Fact]
    public void Values()
    {
        // Arrange
        var entityList = new EntityList<int, Entity>(e => e.Id);

        // Act
        var entity1 = new Entity
        {
            Id = 1,
            Property = "One"
        };
        entityList.Upsert(entity1);

        var entity2 = new Entity
        {
            Id = 2,
            Property = "Two"
        };
        entityList.Upsert(entity2);

        // Assert
        var expected = new List<Entity> { entity1, entity2 };
        Assert.Equal(expected, entityList.Values);
    }

    [Fact]
    public void InsertEntityUsingArrayIndexer()
    {
        // Arrange
        var entityList = new EntityList<int, Entity>(e => e.Id);

        int addedEvents = 0;
        int updatedEvents = 0;

        entityList.Added += (sender, e) =>
        {
            addedEvents++;
        };
        entityList.Updated += (sender, e) =>
        {
            updatedEvents++;
        };

        // Act
        var entity1 = new Entity
        {
            Id = 1,
            Property = "One"
        };
        entityList[1] = entity1;

        // Assert
        Assert.Equal(1, addedEvents);
        Assert.Equal(0, updatedEvents);

        Assert.Single(entityList);
        Assert.Equal(entity1, entityList[1]);
    }

    [Fact]
    public void UpdateEntityUsingArrayIndexer()
    {
        // Arrange
        var entityList = new EntityList<int, Entity>(e => e.Id);

        int addedEvents = 0;
        int updatedEvents = 0;

        entityList.Added += (sender, e) =>
        {
            addedEvents++;
        };
        entityList.Updated += (sender, e) =>
        {
            updatedEvents++;
        };

        // Act
        var entity1 = new Entity
        {
            Id = 1,
            Property = "One"
        };
        entityList[1] = entity1;
        entityList[1] = entity1;

        // Assert
        Assert.Equal(1, addedEvents);
        Assert.Equal(1, updatedEvents);

        Assert.Single(entityList);
        Assert.Equal(entity1, entityList[1]);
    }

    [Fact]
    public void Clear()
    {
        // Arrange
        var entityList = new EntityList<int, Entity>(e => e.Id);

        var entity1 = new Entity
        {
            Id = 1,
            Property = "One"
        };
        entityList.Upsert(entity1);

        var entity2 = new Entity
        {
            Id = 2,
            Property = "Two"
        };
        entityList.Upsert(entity2);

        int removedEvents = 0;

        entityList.Removed += (sender, e) =>
        {
            removedEvents++;
        };

        // Act
        entityList.Clear();

        // Assert
        Assert.Equal(2, removedEvents);
        Assert.Empty(entityList.Values);
    }
}
