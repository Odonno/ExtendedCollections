using System;
using System.Collections.Generic;

namespace ExtendedCollections
{
    public class EntityList<TKey, TEntity> : List<TEntity>
    {
        private readonly Func<TEntity, TKey> _selectKey;

        public EntityList(Func<TEntity, TKey> selectKey)
        {
            if (selectKey == null)
            {
                throw new ArgumentNullException(nameof(selectKey));
            }

            _selectKey = selectKey;
        }

        public void Upsert(TEntity entity)
        {
            var key = _selectKey(entity);
            Remove(key);
            Add(entity);
        }

        public void UpsertRange(IEnumerable<TEntity> collection)
        {
            foreach (var entity in collection)
            {
                Upsert(entity);
            }
        }

        public bool Exists(TKey key)
        {
            return Exists(e => _selectKey(e).Equals(key));
        }

        public TEntity Find(TKey key)
        {
            return Find(e => _selectKey(e).Equals(key));
        }

        public bool Remove(TKey key)
        {
            return RemoveAll(e => _selectKey(e).Equals(key)) > 0;
        }
    }
}
