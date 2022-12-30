using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace ExtendedCollections
{
    /// <summary>
    /// A list to store unique items based on a key. 
    /// A mix between a <see cref="List{T}"/> and a <see cref="Dictionary{TKey, TEntity}"/>.
    /// </summary>
    /// <typeparam name="TKey">The type of the key identifier of each entity.</typeparam>
    /// <typeparam name="TEntity">The type of entity stored.</typeparam>
    public class EntityList<TKey, TEntity> : IEnumerable<TEntity>
    {
        private readonly Func<TEntity, TKey> _selectKey;
        private readonly ConcurrentDictionary<TKey, TEntity> _entities;

        public TEntity this[TKey key] 
        { 
            get 
            {
                return _entities[key];
            }
            set 
            { 
                _entities[key] = value;
            }
        }

        /// <summary>
        /// Gets a collection containing the keys in the list.
        /// </summary>
        public ICollection<TKey> Keys => _entities.Keys;

        /// <summary>
        /// Gets a collection containing the values in the list.
        /// </summary>
        public ICollection<TEntity> Values => _entities.Values;

        /// <summary>
        /// Gets the number of entities contained in the list.
        /// </summary>
        public int Count => _entities.Count;

        /// <summary>
        /// Creates a new instance of a <see cref="EntityList{TKey, TEntity}"/>.
        /// </summary>
        /// <param name="selectKey">A selector function to retrieve the key of an entity.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="selectKey"/> argument is null.</exception>
        public EntityList(Func<TEntity, TKey> selectKey)
        {
            if (selectKey is null)
            {
                throw new ArgumentNullException(nameof(selectKey));
            }

            _selectKey = selectKey;
            _entities = new ConcurrentDictionary<TKey, TEntity>();
        }

        public IEnumerator<TEntity> GetEnumerator()
        {
            return _entities.Values.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Insert an entity in the list, or update if it already exists.
        /// </summary>
        /// <param name="entity">The entity to insert or update.</param>
        public void Upsert(TEntity entity)
        {
            var key = _selectKey(entity);
            _entities.AddOrUpdate(key, entity, (_, __) => entity);
        }
        /// <summary>
        /// Insert a collection of entity in the list, or update any entity if it already exists.
        /// </summary>
        /// <param name="collection">The collection of entity to insert or update.</param>
        public void Upsert(IEnumerable<TEntity> collection)
        {
            foreach (var entity in collection)
            {
                Upsert(entity);
            }
        }

        /// <summary>
        /// Detects if an entity with the given key exists.
        /// </summary>
        /// <param name="key">The key identifier of an entity.</param>
        /// <returns>true if an entity with the given key exists.</returns>
        public bool Exists(TKey key)
        {
            return _entities.ContainsKey(key);
        }

        /// <summary>
        /// Find an entity with the given key, if it exists.
        /// </summary>
        /// <param name="key">The key identifier of an entity.</param>
        /// <returns>The entity that matches the key, if found; otherwise, the default value for type <typeparamref name="TEntity"/>.</returns>
        public TEntity Find(TKey key)
        {
            _entities.TryGetValue(key, out var entity);
            return entity;
        }

        /// <summary>
        /// Remove an entity with the given key, if it exists.
        /// </summary>
        /// <param name="key">The key identifier of an entity.</param>
        /// <returns>true if the entity was removed.</returns>
        public bool Remove(TKey key)
        {
            return _entities.TryRemove(key, out _);
        }

        /// <summary>
        /// Removes all elements from the list.
        /// </summary>
        public void Clear()
        {
            _entities.Clear();
        }
    }
}
