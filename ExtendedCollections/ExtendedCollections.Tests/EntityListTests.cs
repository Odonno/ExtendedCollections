using System.Collections.Generic;
using Xunit;

namespace ExtendedCollections.Tests
{
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

            // Act
            entityList.Remove(1);

            // Assert
            Assert.Empty(entityList);
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
        public void SetEntityUsingArrayIndexer()
        {
            // Arrange
            var entityList = new EntityList<int, Entity>(e => e.Id);

            // Act
            var entity1 = new Entity
            {
                Id = 1,
                Property = "One"
            };
            entityList[1] = entity1;

            // Assert
            Assert.Single(entityList);
            Assert.Equal(entity1, entityList[1]);
        }
    }
}
