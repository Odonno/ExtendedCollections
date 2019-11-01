using System;
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

            // Act
            var entity1 = new Entity
            {
                Id = 1,
                Property = "Created"
            };
            entityList.Upsert(entity1);
            var newEntity1 = new Entity
            {
                Id = 1,
                Property = "Updated"
            };
            entityList.Upsert(newEntity1);

            // Assert
            Assert.Single(entityList);
            Assert.Equal(1, entityList[0].Id);
            Assert.Equal("Updated", entityList[0].Property);
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
    }
}
