using System;
using System.Collections.Generic;
using System.Linq;

namespace UNKO.Utils
{
    public class SimplePool<T>
        where T : class
    {
        public int instanceCount => _allInstance.Count;
        public int useCount => _use.Count;
        public int notUseCount => _notUse.Count;

        protected List<T> _allInstance = new List<T>();
        protected HashSet<T> _use = new HashSet<T>();
        protected List<T> _notUse = new List<T>();
        protected T _originItem { get; private set; }

        public SimplePool(int initializeSize = 0)
        {
            Init(Activator.CreateInstance<T>(), initializeSize);
        }

        public SimplePool(T originItem, int initializeSize = 0)
        {
            Init(originItem, initializeSize);
        }

        public T Spawn()
        {
            T spawnItem = null;
            if (_notUse.Count > 0)
            {
                int lastIndex = _notUse.Count - 1;
                spawnItem = _notUse[lastIndex];
                _notUse.RemoveAt(lastIndex);
            }
            else
            {
                spawnItem = OnRequireNewInstance(_originItem);
                _allInstance.Add(spawnItem);
            }

            OnSpawn(spawnItem);
            _use.Add(spawnItem);
            return spawnItem;
        }

        public void DeSpawn(T item)
        {
            if (_use.Contains(item) == false)
                return;

            OnDespawn(item);
            _use.Remove(item);
            _notUse.Add(item);
        }

        public void DeSpawnAll()
        {
            while (_use.Count > 0)
                DeSpawn(_use.Last());
        }

        protected virtual T OnRequireNewInstance(T originItem) => Activator.CreateInstance<T>();
        protected virtual void OnSpawn(T spawnTarget) { }
        protected virtual void OnDespawn(T despawnTarget) { }

        private void Init(T originItem, int initializeSize)
        {
            _originItem = originItem;

            for (int i = 0; i < initializeSize; i++)
                Spawn();
            DeSpawnAll();
        }
    }
}
